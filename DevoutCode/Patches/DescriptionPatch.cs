using System.Reflection;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Patches.Vars;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Patches;

[HarmonyPatch(typeof(CardModel))]
public static class DescriptionPatch {
    static MethodBase TargetMethod() => AccessTools.DeclaredMethod("MegaCrit.Sts2.Core.Models.CardModel:GetDescriptionForPile", parameters: [typeof(PileType), AccessTools.Inner(typeof(CardModel), "DescriptionPreviewType"), typeof(Creature)]);
    public static void Postfix(CardModel __instance, ref string __result) {
        int heavy;
        if (StrengthPatch.GetHeavy(__instance, out heavy, out HeavyVar variable)) {
            if (variable.keyword) {
                switch (heavy) {
                    case > 1: __result += $"\n[gold]Heavy[/gold] {heavy}."; break;
                    case 0: __result += "\n[gold]Light[/gold]."; break;
                }
            } else {
                string text = heavy switch {
                    0 => "LIGHT",
                    _ => "HEAVY"
                };
                LocString loc = LocString.GetIfExists("static_hover_tips", $"DEVOUT-{text}.smartDescription");
                loc.Add("Heavy", heavy);
                __result += "\n" + loc.GetFormattedText().Replace("[blue]", "").Replace("[/blue]", "").Replace("×", " times");
            }
        }
        if (__instance is DevoutCard card)
            if (!(card.GetFlip(out Type? flip) == null || card.Description.GetFormattedText().Contains("[gold]Flip[/gold]")))
                __result = "[gold]Flip[/gold].\n" + __result;

        foreach (string keyword in new[] { "Temporary", "Rhythmic", "Flip" }) {
            string start = $"[gold]{keyword}[/gold].";
            string end = $"[color=#ffffff00][font_size=1]{keyword}[/font_size][/color]";
            if (__result.Contains(start))
                __result = __result.Replace(start + "\n", end).Replace("\n" + start, end);
        }
    }
}