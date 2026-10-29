using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Relics.Starter.Blood;

public class MilkOfLife : BloodOfTheGods {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RegenPower>(5m)];
}