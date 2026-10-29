using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Forms;

public class SuccubusFormPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(13)];

    public override async Task AfterDamageReceived(PlayerChoiceContext context, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? source) {
        if (dealer == Applier && dealer != target && props.IsPoweredAttack())
            await Bind(target);
    }
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext context, PowerModel power, decimal amount, Creature? applier, CardModel? source) {
        if (power is CharmedPower)
            if (applier.HasPower<SuccubusFormPower>())
                await applier.GetPower<SuccubusFormPower>().Bind(power.Owner);
    }

    public async Task Bind(Creature? creature) {
        if (creature != null)
            await PowerCmd.Apply<BraidboundPower>(new ThrowingPlayerChoiceContext(), creature, Amount, Owner, null);
    }

    public override async Task AfterBraidboundStun(BraidboundPower power) {
        await GeneralCmd.Lifesteal(new ThrowingPlayerChoiceContext(), null, Owner, DynamicVars.Heal.BaseValue, null);
    }
}