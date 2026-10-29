using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Powers.Buffs.Guards;

public abstract class GuardPower : DevoutPower, IHasSecondAmount {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Primary", Amount), new IntVar("Secondary", 0)];
    
    public int SecondAmount {
        get => (int) DynamicVars["Secondary"].BaseValue;
        set => DynamicVars["Secondary"].BaseValue = value;
    }
    public string GetSecondAmount() => SecondAmount != 0 ? SecondAmount.ToString() : "";
}