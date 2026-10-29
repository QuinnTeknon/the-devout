using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Flagellance;

public abstract class TemporaryFlagellantPower : DevoutPower, ITemporaryPower {
    private bool _shouldIgnoreNextInstance;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerType Type => !IsPositive ? PowerType.Debuff : PowerType.Buff;
    protected bool IsPositive => true;
    private int Sign => !IsPositive ? -1 : 1;

    public abstract AbstractModel OriginModel { get; }
    public PowerModel InternallyAppliedPower => ModelDb.Power<FlagellantPower>();
    
    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;

    public override async Task BeforeApplied(Creature target, Decimal amount, Creature? applier, CardModel? source) {
        if (_shouldIgnoreNextInstance) {
            _shouldIgnoreNextInstance = false;
        } else {
            FlagellantPower flagellant = await PowerCmd.Apply<FlagellantPower>(new ThrowingPlayerChoiceContext(), target, Sign * amount, applier, source, true);
        }
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext context, PowerModel power, Decimal amount, Creature? applier, CardModel? source) {
        if (amount == Amount)
            return;
        if (_shouldIgnoreNextInstance) {
            _shouldIgnoreNextInstance = false;
        } else {
            FlagellantPower flagellant = await PowerCmd.Apply<FlagellantPower>(context, Owner, Sign * amount, applier, source, true);
        }
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (!participants.Contains(Owner))
            return;
        Flash();
        await PowerCmd.Remove(this);
        FlagellantPower flagellant = await PowerCmd.Apply<FlagellantPower>(context, Owner, -Sign * Amount, Owner, null);
    }
}