using System.Reflection;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Extensions.Resource;
using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;

namespace Devout.DevoutCode.Patches;

public static class RelicReplacementPatch {
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.DynamicDescription), MethodType.Getter)]
    public class DynamicDescription {
        public static void Postfix(RelicModel __instance, ref LocString __result) {
            if (__instance is { IsMutable: true, Owner.Character: Character.Devout }) {
                LocString? description = LocString.GetIfExists("relic_replacements", __instance.Id.Entry + ".description");
                if (description == null)
                    return;
                foreach (KeyValuePair<string, object> var in __result.Variables)
                    description.Add(var.Key, var.Value?.ToString() ?? "");
                __result = description;
            }
        }
    }
    [HarmonyPatch(typeof(RelicModel), nameof(RelicModel.DynamicEventDescription), MethodType.Getter)]
    public class DynamicEventDescription {
        public static void Postfix(RelicModel __instance, ref LocString __result) {
            if (__instance is { IsMutable: true, Owner.Character: Character.Devout }) {
                LocString? description = LocString.GetIfExists("relic_replacements", __instance.Id.Entry + ".eventDescription");
                if (description == null)
                    return;
                foreach (KeyValuePair<string, object> var in __result.Variables)
                    description.Add(var.Key, var.Value?.ToString() ?? "");
                __result = description;
            }
        }
    }
}