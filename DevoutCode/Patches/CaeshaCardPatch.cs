using BaseLib.Abstracts;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Rewards.Other.Common;
using Devout.DevoutCode.Cards.Rewards.Other.Rare.Heart;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Extensions.Resource;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Saves;

namespace Devout.DevoutCode.Patches;

public class DevoutCardPlayPatch {
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.CanPlay), [typeof(UnplayableReason), typeof(AbstractModel)], [ArgumentType.Out, ArgumentType.Out])]
    public class CanPlay {
        public static void Postfix(CardModel __instance, ref bool __result, UnplayableReason reason) {
            if (__instance is DevoutCard card) {
                __result &= card.CanPlayInternal();
                TextureRect? slash = NCard.FindOnTable(__instance)?.GetNode<TextureRect>("%UnplayableEnergyIcon") ?? null;
                if (slash?.Visible ?? false) {
                    if (card.unplayable != DevoutCard.UnplayableReason.NONE && reason == UnplayableReason.None) {
                        slash.Visible = false;
                    }
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(NCardPlay), nameof(NCardPlay.CancelPlayCard))]
    public class CancelPlay {
        public static void Postfix(NCardPlay __instance) {
            CardModel? card = __instance.Holder.CardModel;
            if (card is DevoutCard devoutCard) {
                devoutCard.DisplayInsufficiencyReason();
            }
        }
    }
}

public class DevoutCardCostPatch {
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisTurn))]
    public class ThisTurn {
        public static async void Postfix(CardModel __instance) {
            if (__instance is DevoutCard card) {
                await Task.Run(() => {
                    card.piety.SetThisTurn(0);
                    card.penance.SetThisTurn(0);
                });
                await Task.Run(() => NCard.FindOnTable(card)?.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal));
            }
        }
    }
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisCombat))]
    public class ThisCombat {
        public static async void Postfix(CardModel __instance) {
            if (__instance is DevoutCard card) {
                await Task.Run(() => {
                    card.piety.SetThisCombat(0);
                    card.penance.SetThisCombat(0);
                });
                await Task.Run(() => NCard.FindOnTable(card)?.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal));
            }
        }
    }
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.CostsEnergyOrStars))]
    public class HasCost {
        public static void Postfix(CardModel __instance, ref bool __result) {
            if (__instance is DevoutCard card) {
                __result |= card.piety.Canonical > 0;
                __result |= card.penance.Canonical > 0;
            }
        }
    }
    
    public class ModifyCost {
        [HarmonyPatch(typeof(CustomResourceCost<SpireResource>), nameof(CustomResourceCost<SpireResource>.GetWithModifiers))]
        public class Get {
            public static void Postfix(ref int __result, CustomResourceCost<SpireResource> __instance, CardModel ____card, List<LocalCostModifier> ____localModifiers) {
                //MainFile.Logger.Info($"RESOURCE_COST({____card.GetType().ToString().Split(".").Last()}):\n - {String.Join("\n - ", ____localModifiers.Select(m => $"{m.Amount} ({m.Type.ToString()}, Expires: {m.Expiration})"))}");
            }
        }
        
        [HarmonyPatch(typeof(CustomResourceCost<SpireResource>), nameof(CustomResourceCost<SpireResource>.SetThisTurn))]
        public class Turn {
            public static void Postfix(int cost, CustomResourceCost<SpireResource> __instance, CardModel ____card, List<LocalCostModifier> ____localModifiers) {
                MainFile.Logger.Info($"Made {____card.GetType().ToString().Split(".").Last()}'s {((SpireResourceCost) __instance).name} cost {cost} this turn.");
                MainFile.Logger.Info($" - {String.Join("\n - ", ____localModifiers.Select(m => $"{m.Amount} ({m.Type.ToString()}, Expires: {m.Expiration})"))}");
            }
        }
        [HarmonyPatch(typeof(CustomResourceCost<SpireResource>), nameof(CustomResourceCost<SpireResource>.SetThisCombat))]
        public class Combat {
            public static void Postfix(int cost, CustomResourceCost<SpireResource> __instance, CardModel ____card, List<LocalCostModifier> ____localModifiers) {
                MainFile.Logger.Info($"Made {____card.GetType().ToString().Split(".").Last()}'s {((SpireResourceCost) __instance).name} cost {cost} this combat.");
                MainFile.Logger.Info($" - {String.Join("\n - ", ____localModifiers.Select(m => $"{m.Amount} ({m.Type.ToString()}, Expires: {m.Expiration})"))}");
            }
        }
    }
}