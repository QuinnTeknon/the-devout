using BaseLib.Extensions;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Angurvadal;
using Devout.DevoutCode.Cards.Starters;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.Commands;

public static class AngurCmd {
    public static async Task Unsheathe(Player player, AbstractModel? source = null, bool upgraded = false, TechniqueEnchant? technique = null, Vector2? damage = null, Vector2? block = null) {
        damage ??= Vector2.Right;
        block ??= Vector2.Right;
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        GetAngurInstances(player, includeExhausted: false, out List<Angur> blades);
        Angur angur;
        if (blades.Count == 0) {
            angur = player.Creature.CombatState.CreateCard<Angur>(player);
            angur.unsheathed = true;
            await CardPileCmd.AddGeneratedCardToCombat(angur, PileType.Hand, player);
            blades.Add(angur);
        } else {
            angur = blades[0];
            await CardPileCmd.Add(blades, PileType.Hand, CardPilePosition.Top, source);
        }
        if (player.HasPower<SwordOfEdenPower>()) { await CardCmd.TransformTo<Angurvadal>(angur, CardPreviewStyle.EventLayout); } else { await CardCmd.TransformTo<Angur>(angur, CardPreviewStyle.EventLayout); }
        angur = (Angur) (await (player.HasPower<SwordOfEdenPower>() ? CardCmd.TransformTo<Angurvadal>(angur, CardPreviewStyle.EventLayout) : CardCmd.TransformTo<Angur>(angur, CardPreviewStyle.EventLayout))).Value.cardAdded;
        if (upgraded) { CardCmd.Upgrade(angur); }
        if (technique != null) {
            TechniqueEnchant tech = (TechniqueEnchant) technique.ToMutable();
            tech.damageMult = (decimal) damage.Value.X;
            tech.DynamicVars["ExtraDamage"].BaseValue += (decimal) damage.Value.Y;
            if (tech.DynamicVars.ContainsKey("Block")) {
                tech.DynamicVars["Block"].BaseValue *= (decimal) block.Value.X;
                tech.DynamicVars["Block"].BaseValue += (decimal) block.Value.Y;
            }
            await GeneralCmd.Enchant(tech, angur, tech.DynamicVars["ExtraDamage"].BaseValue);
            await Cmd.Wait(0.1f);
        }
        await AfterAngurUnsheathed(new ThrowingPlayerChoiceContext(), angur);
    }
    
    public static List<Angur> GetAngurInstances(Player? player, bool includeExhausted, out List<Angur> instances) {
        instances = player != null ? player.PlayerCombatState.AllCards.Where(delegate(CardModel c) {
            if (!c.IsDupe) {
                if (!includeExhausted) {
                    CardPile? pile = c.Pile;
                    if (pile == null) {
                        return true;
                    }
                    return pile.Type != PileType.Exhaust;
                }
                return true;
            }
            return false;
        }).OfType<Angur>().ToList() : [];
        return instances;
    }
    
    public static Task AfterAngurPlayed(PlayerChoiceContext context, CardPlay play, Angur angur) {
        ICombatState state = angur.CombatState;
        foreach (AbstractModel model in state.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card) {
                    card.AfterAngurPlayed(context, play, angur);
                } else if (model is DevoutPower power) {
                    power.AfterAngurPlayed(context, play, angur);
                } else if (model is DevoutRelic relic) {
                    relic.AfterAngurPlayed(context, play, angur);
                }
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }
    
    private static Task AfterAngurUnsheathed(PlayerChoiceContext context, Angur? angur) {
        if (angur != null) {
            ICombatState state = angur.CombatState;
            foreach (AbstractModel model in state.IterateHookListeners()) {
                if (model is DevoutCard or DevoutPower or DevoutRelic) {
                    if (model is DevoutCard card) {
                        card.AfterAngurUnsheathed(context, angur);
                    } else if (model is DevoutPower power) {
                        power.AfterAngurUnsheathed(context, angur);
                    } else if (model is DevoutRelic relic) {
                        relic.AfterAngurUnsheathed(context, angur);
                    }
                    model.InvokeExecutionFinished();
                }
            }
        }
        return Task.CompletedTask;
    }
}