using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Extensions.Resource;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Patches;

public static class EnchantmentPatch {
    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.DynamicDescription), MethodType.Getter)]
    public class DescriptionPatch {
        [HarmonyPostfix]
        public static void SymboliseResources(EnchantmentModel __instance, ref LocString __result) {
            if (__instance is DevoutEnchantment) {
                LocString description = __result;
                foreach (string var in new[]{ "piety", "penance" }) {
                    LocString dict = NResourceCounter.getLocString(var).Item1;
                    description.Add(dict.Variables.First().Key, dict.Variables.First().Value.ToString());
                }
                __result = description;
            }
        }
    }
}