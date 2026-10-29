using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Relics.Starter;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.RestSite;

namespace Devout.DevoutCode.Patches.Events;

[HarmonyPatch(typeof(RestSiteOption), "IconPath",  MethodType.Getter)]
public class RestSiteOptionPatch {
    public static void Postfix(RestSiteOption __instance, ref string __result) {
        if (__instance is not GazeRestSiteOption)
            return;
        __result = $"{__instance.OptionId.ToLowerInvariant()}_option.png".RestOptionImagePath() ?? __result;
    }
}