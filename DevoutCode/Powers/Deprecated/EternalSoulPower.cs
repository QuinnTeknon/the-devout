using Devout.DevoutCode.Extensions.Resource;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Powers.Buffs;

public class EternalSoulPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterResourceSpent(SpireResource res, int amount, Player spender, object? info = null) {
        if (CheckResource("piety", res, spender)) {
            if (spender.Creature == Owner && amount > 0) {
                var Threshold = DynamicVars["Piety"].IntValue;
                Data data = GetInternalData<Data>();
                var spent = data.spent + amount;
                if (Amount >= Threshold || (spent % Threshold < data.spent % Threshold)) {
                    Flash();
                    data.spent = spent;
                    await PlayerCmd.GainEnergy(Amount * (spent / Threshold), Owner.Player);
                }
                InvokeDisplayAmountChanged();
            }
        }
    }
    
    private class Data {
        public int spent;
    }

    public override int DisplayAmount => DynamicVars["Piety"].IntValue - GetInternalData<Data>().spent % DynamicVars["Piety"].IntValue;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.ForEnergy(this)];

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Piety", 6)];

    protected override object InitInternalData() {
        return new Data();
    }
}