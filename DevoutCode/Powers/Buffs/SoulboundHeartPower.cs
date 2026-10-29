using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs;

public class SoulboundHeartPower() : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? source, CardPlay? play) {
        if (target != Owner)
            return 1m;
        if (!props.IsPoweredAttack())
            return 1m;
        if (target.GetPower<BraidboundPower>()?.Owner != Owner)
            return 1m;
        return 0.5m;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants) {
        if (side == CombatSide.Enemy)
            await PowerCmd.Remove(this);
    }
}