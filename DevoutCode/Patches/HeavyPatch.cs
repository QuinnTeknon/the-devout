using System.Reflection;
using BaseLib.Cards.Variables;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Patches;

[HarmonyPatch(typeof(StrengthPower))]
public static class StrengthPatch {
    static MethodBase TargetMethod() => AccessTools.DeclaredMethod(typeof(StrengthPower), "ModifyDamageAdditive");
    
    [HarmonyPostfix]
    public static void ModifyDamage(CardModel? cardSource, ref decimal __result) {
        int heavy, titanicus;
        GetHeavy(cardSource, out heavy, out HeavyVar? variable);
        GetPower<TitanicusPower>(cardSource, out titanicus);
        __result *= variable == null ? 1 : heavy + titanicus;
    }

    public static bool GetHeavy(CardModel? card, out int heavy, out HeavyVar? variable) {
        heavy = 0;
        variable = null;
        bool b = false, e = false;
        if (card != null) {
            b = card.DynamicVars.TryGetValue(HeavyVar.Key, out var vb);
            int bass = b ? vb.IntValue : 1;
            heavy += bass;
            variable ??= b ? (HeavyVar) vb : null;
            if (card.Enchantment != null) {
                e = card.Enchantment.DynamicVars.TryGetValue(HeavyVar.Key, out var ve);
                int enchant = e ? ve.IntValue : 0;
                heavy += enchant - 1;
                variable ??= e ? (HeavyVar) ve : null;
            }
        }
        return b || e;
    }
    
    public static int GetPower<T>(CardModel? card, out int power) where T : PowerModel {
        power = 0;
        if (card != null)
            power = card.Owner.Creature.GetPowerAmount<T>();
        return power;
    }
}

[HarmonyPatch(typeof(CliodnaStrengthPower))]
public static class CliodnaStrengthPatch {
    static MethodBase TargetMethod() => AccessTools.DeclaredMethod(typeof(CliodnaStrengthPower), "ModifyDamageAdditive");
    public static void Postfix(CardModel? cardSource, ref decimal __result) => StrengthPatch.ModifyDamage(cardSource, ref __result);
}