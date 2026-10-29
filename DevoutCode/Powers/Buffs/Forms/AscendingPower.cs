using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Forms;

public class AscendingPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => true;
    public bool IsGrounded;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("DamageDecrease", 50m), new("Wings", Amount)];
    
    public virtual Task Crash(PlayerChoiceContext context) => Task.CompletedTask;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? source, CardPlay? play) {
        if (target != Owner)
            return 1m;
        if (!props.IsPoweredAttack())
            return 1m;
        if (dealer == null)
            return 1m;
        if (IsGrounded)
            return 1m;
        return DynamicVars["DamageDecrease"].BaseValue / 100m;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext context, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? source) {
        if (target == Owner && result.UnblockedDamage != 0 && props.IsPoweredAttack()) {
            await PowerCmd.Decrement(this);
            if (Amount <= 0) {
                if (!IsGrounded) {
                    Flash();
                    IsGrounded = true;
                    await PowerCmd.Apply<RingingPower>(context, Owner, 2, Owner, null);
                    Crash(context);
                    await PowerCmd.Decrement(this);
                    await Cmd.Wait(0.25f);
                }
            }
        }
    }
}