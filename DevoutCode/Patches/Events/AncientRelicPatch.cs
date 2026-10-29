using System.Reflection;
using Devout.DevoutCode.Relics.Ancient.Nonupeipe;
using Devout.DevoutCode.Relics.Ancient.Tanx;
using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;

namespace Devout.DevoutCode.Patches.Events;

public static class AncientRelicPatch {
    [HarmonyPatch(typeof(Tanx), "BaseOptionPool",  MethodType.Getter)]
    public class TanxRelics {
        public static void Postfix(Tanx __instance, ref IEnumerable<EventOption> __result) {
            __result = __result.AddNewRelic<Tanx, Character.Devout, EndlessAppetite>(__instance);
        }
    }
    [HarmonyPatch(typeof(Nonupeipe), "OptionPool",  MethodType.Getter)]
    public class NonupeipeRelics {
        public static void Postfix(Nonupeipe __instance, ref IEnumerable<EventOption> __result) {
            __result = __result.AddNewRelic<Nonupeipe, Character.Devout, SoulboundHeart>(__instance);
        }
    }

    public static IEnumerable<EventOption> AddNewRelic<A, C, R>(this IEnumerable<EventOption> options, A ancient) where A : AncientEventModel where C : CharacterModel where R : RelicModel {
        if (ancient is { IsMutable: true, Owner.Character: not C })
            return options;
        MethodInfo? method = AccessTools.Method(typeof(A), "RelicOption", generics: [typeof(R)], parameters: [typeof(string), typeof(string)]);
        EventOption? relic = (EventOption?) method?.Invoke(ancient, ["INITIAL", null]);
        IEnumerable<EventOption> result = relic != null ? options.ToList().Append(relic) : options.ToList();
        MainFile.Logger.Info($"RELIC_TEST: {ancient.GetType().ToString().Split(".").Last()} ({method == null} | {relic == null}):\n - {String.Join("\n - ", result.ToList())}");
        return result;
    }
}