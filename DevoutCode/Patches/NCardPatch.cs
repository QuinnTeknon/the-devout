using BaseLib.Abstracts;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Rewards.Other.Rare.Heart;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Character;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Devout.DevoutCode.Patches;

[HarmonyPatch(typeof(NCard), nameof(NCard._Ready))]
public class NCardReadyPatch {
    public static void Postfix(NCard __instance) {//.455f, .415f
        if (__instance.Model is DevoutCard card && new[]{CardType.Attack, CardType.Skill, CardType.Power}.Contains(card.Type)) {
            MainFile.Logger.Info($"NCARD[{__instance}]: {__instance.Model}");
            createResourceIcon("piety", (new Vector2(-1, -40), 180, 1.5f, -1), (new Vector2(0.04f, 0.005f), new Vector2(-0.01f, 0)), __instance);
            createResourceIcon("penance", (new Vector2(1, 36), -45, 1.5f, 1), (new Vector2(0, 0), new Vector2(0.23f, 0)), __instance);
        }
    }
    
    public static Control createResourceIcon(string title, (Vector2, int, float, int) transform, (Vector2, Vector2) text, NCard instance) {
        var visualScene = ResourceLoader.Load<PackedScene>($"res://Devout/scenes/ui/combat/resources/{title}.tscn");
        if (visualScene != null) {
            text.Item1 += new Vector2(0.415f, 0);
            Control counter = visualScene.Instantiate<Control>();
            counter.Name = $"_{title.Capitalize()}Icon";
            counter.Scale *= 0.6f * transform.Item3;
            counter.Position = new Vector2(-142, -243) + transform.Item1;
            counter.MouseFilter = Control.MouseFilterEnum.Ignore;
            instance.GetChild(0).AddChildSafely(counter);
            instance.GetChild(0).MoveChild(counter, instance.GetNode<TextureRect>("%EnergyIcon").GetIndex() + transform.Item4);
            //MainFile.Logger.Info($"Descendants: {icon.FindChildren("*", owned: false).Select(child => $"{child.Name}{(child is Control ? $"{(((Control) child).Visible ? " ⛊" : " ⛉")}: {((Control) child).GlobalPosition}" : "")}").ToArray().Join()}");
            TextureRect icon = counter.GetNode<TextureRect>("Icon");
            icon.RemoveChild(icon.GetNode<Control>("RotationLayers"));
            icon.Position -= icon.Size * text.Item1;
            icon.Rotation = Single.DegreesToRadians(transform.Item2);
            icon.Scale /= transform.Item3;
            Control label = counter.GetNode<Control>("Count");
            var pos = label.Position;
            label.Position -= icon.Size * (text.Item1 + text.Item2) - new Vector2(43, 0) + pos / 2;
            counter.Visible = true;
            return counter;
        }
        return null;
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateVisuals))]
public class NCardUpdatePatch {
    public static void Postfix(NCard __instance, PileType pileType, TextureRect ____energyIcon, Creature? ____previewTarget) {
        if (__instance.Model is DevoutCard card) {
            updateResourceIcon("piety", __instance, pileType, card.piety.Canonical != 0 || card.piety.CostsX);
            updateResourceIcon("penance", __instance, pileType, card.penance.Canonical != 0 || card.penance.CostsX);
            MegaRichTextLabel text = __instance.GetNode<MegaRichTextLabel>("CardContainer/DescriptionLabel");
            if (__instance.Model is Angur || card.GetFlip(out Type? _) != null) {
                ____energyIcon.Texture = ResourceLoader.Load<Texture2D>("charui/big_energy.png".ImagePath());
                if (__instance.Model.IsMutable) {
                    if (__instance.Model is Angur angur) {
                        angur.target = ____previewTarget;
                        try {
                            text.SetTextAutoSize("[center]" + angur.GetDescriptionForPile(angur.Pile?.Type ?? PileType.None, angur.target) + "[/center]");
                        } catch (Exception e) { MainFile.Logger.Error($"{e.Source}: {e.Message}"); }
                    }
                }
            }
            text.Text = text.Text.replaceLocVars("piety", __instance.Model).replaceLocVars("penance", __instance.Model);
            //MainFile.Logger.Info($"LOC_VAR[{__instance.Model.GetType().ToString().Split(".").Last()}]: '{text.Text}'");
        }
    }
    
    public static Control updateResourceIcon(string title, NCard instance, PileType pileType, bool condition) {
        bool found = instance.GetChild(0).HasNode($"_{title.Capitalize()}Icon");
        string[] ss = instance.Model.GetType().ToString().Split(".");
        //MainFile.Logger.Info($"{ss[ss.Length - 1]} | '{title}' is '{found}' | Descendants: {instance.FindChildren("*", owned: false).Select(child => $"{child.Name}{(child is Control ? $"{(((Control) child).Visible ? " ⛊" : " ⛉")}: {((Control) child).GlobalPosition}" : "")}").ToArray().Join()}");
        if (found) {
            Control counter = instance.GetChild(0).GetNode<Control>($"_{title.Capitalize()}Icon");
            counter.Visible = false;
            if (condition) {
                SpireResourceCost? cost = (SpireResourceCost?) AccessTools.Field(typeof(DevoutCard), title)?.GetValue(instance.Model);
                int? value = instance.Model.IsMutable ? cost.Get() : cost.Canonical;
                MainFile.Logger.Info($"{instance.Model.GetType().ToString().Split(".").Last()}'s {title} is {value}.");
                if (cost.Canonical != 0 || cost.CostsX) {
                    RichTextLabel label = counter.GetNode<RichTextLabel>("Count/Label");
                    label.Text = cost.CostsX ? "X" : value.ToString();
                    label.Position = new Vector2(-24, 0) * (label.Text.Length - 1) / 2 - (cost.CostsX ? new Vector2(1, 0) : Vector2.Zero);
                    //label.AddThemeFontOverride(ThemeConstants.Label.Font, ("res://themes/kreon_bold_shared.tres"));
                    if (instance.Model.CombatState != null) {
                        UpdateCostColor(title, label, cost, instance, pileType);
                    } else {
                        label.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, Colors.White);
                        label.AddThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor, Colors.Black);
                    }
                    counter.Visible = true;
                }
            }
            return counter;
        }
        return null;
    }
    
    private static void UpdateCostColor(string title, RichTextLabel label, SpireResourceCost? cost, NCard instance, PileType pileType) {
        Color color1 = Colors.White;
        Color color2 = instance.Model.Owner.Character.EnergyLabelOutlineColor;
        if (pileType == PileType.Hand) {
            CardCostColor costColor = GetCostColor(title, cost, instance, instance.Model.CombatState);
            color1 = GetCostTextColorInHand(costColor, color1);
            color2 = GetCostOutlineColorInHand(costColor, color2);
        }
        label.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, color1);
        label.AddThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor, color2);
    }
    
    private static Color GetCostTextColorInHand(CardCostColor costColor, Color defaultColor) {
        return costColor switch {
            CardCostColor.Unmodified => defaultColor, 
            CardCostColor.Increased => StsColors.energyBlue, 
            CardCostColor.Decreased => StsColors.green, 
            CardCostColor.InsufficientResources => StsColors.red, 
            _ => throw new ArgumentOutOfRangeException("costColor", costColor, null), 
        };
    }
    
    private static Color GetCostOutlineColorInHand(CardCostColor costColor, Color defaultColor) {
        return costColor switch {
            CardCostColor.Unmodified => defaultColor, 
            CardCostColor.Increased => StsColors.energyBlueOutline, 
            CardCostColor.Decreased => StsColors.energyGreenOutline, 
            CardCostColor.InsufficientResources => StsColors.unplayableEnergyCostOutline, 
            _ => throw new ArgumentOutOfRangeException("costColor", costColor, null), 
        };
    }
    
    public static CardCostColor GetCostColor(string title, SpireResourceCost? cost, NCard instance, ICombatState? state) {
        if (state != null) {
            if (cost.CostsX)
                return CardCostColor.Unmodified;
            Player player = instance.Model.Owner;
            FlagellantPower? flagellant = player.Creature.GetPower<FlagellantPower>();
            if (flagellant == null)
                return CardCostColor.InsufficientResources;
            SpireResource? resource = (SpireResource?) AccessTools.Field(typeof(FlagellantPower), title)?.GetValue(flagellant) ?? null;
            if (cost.Get() > resource.Amount) {
                return CardCostColor.InsufficientResources;
            }
            switch (cost.Get() - cost.Canonical) {
                case +1: return CardCostColor.Increased;
                case -1: return CardCostColor.Decreased;
            }
        }
        return CardCostColor.Unmodified;
    }
}