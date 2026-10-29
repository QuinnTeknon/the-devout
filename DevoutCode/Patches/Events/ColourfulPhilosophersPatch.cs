using Devout.DevoutCode.Character;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace Devout.DevoutCode.Patches.Events;

public class ColourfulPhilosophersPatch {
    [HarmonyPatch(typeof(ColorfulPhilosophers), "CardPoolColorOrder", MethodType.Getter)]
    public static IEnumerable<CardPoolModel> Postfix(ref IEnumerable<CardPoolModel> __result) => __result.Append(ModelDb.CardPool<DevoutCardPool>());
}