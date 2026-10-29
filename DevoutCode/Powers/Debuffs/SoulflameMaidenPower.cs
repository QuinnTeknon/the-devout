using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Debuffs;

public class SoulflameMaidenPower : DevoutPower {
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext context, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState) {
        if (side == Owner.Side)
            await PowerCmd.Apply<CharmedPower>(context, Owner, Amount, null, null);
    }

    public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == Owner.Side) {
            Flash();
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == Owner.Side)
            await PowerCmd.Decrement(this);
    }
}