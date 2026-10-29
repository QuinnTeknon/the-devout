using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Hooks;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Debuffs;

public class BraidboundPower() : DevoutPower, IHasSecondAmount {
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Damage", 3),
        new IntVar("Stun", 30),
        new DisplayVar<BraidboundPower>("FirstAmount", p => $"{3 * Math.Pow(2, p.count)}"),
        new DisplayVar<BraidboundPower>("SecondAmount", p => $"{30 * Math.Pow(2, p.count)}")];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, StunIntent.GetStaticHoverTip()];
    public override Color AmountLabelColor => active ? Amount >= GetSecondAmount().ToInt() ? Colors.Red : Colors.White : Colors.DimGray;
    public override int DisplayAmount => active ? Amount : 0;
    private bool active = true;
    private bool reset;
    private int count;

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == Owner.Side && active && !reset) {
            SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
            await Cmd.Wait(1);
            await CreatureCmd.Damage(context, Owner, DynamicVars["FirstAmount"].ToString().ToInt(), ValueProp.Unpowered, Owner, null, null);
            if (Amount >= GetSecondAmount().ToInt()) {
                count += 1;
                active = false;
                await CreatureCmd.Stun(Owner);
                await AfterStunned();
                await PowerCmd.ModifyAmount(context, this, -(Amount - 1), null, null);
            }
        }
    }
    
    private Task AfterStunned() {
        foreach (AbstractModel model in Owner.CombatState.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card)
                    card.AfterBraidboundStun(this);
                else if (model is DevoutPower power)
                    power.AfterBraidboundStun(this);
                else if (model is DevoutRelic relic)
                    relic.AfterBraidboundStun(this);
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext context, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource) {
        if (reset && Amount >= 2) {
            reset = false;
            PowerCmd.ModifyAmount(context, this, -1, null, null);
        }
        if (!active) {
            active = true;
            reset = true;
        }
        return Task.CompletedTask;
    }
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context) {
        if (DisplayAmount > 0)
            return [new HealthBarForecastSegment(DynamicVars["FirstAmount"].ToString().ToInt(), new Color(0xEECC00FF), HealthBarForecastDirection.FromRight)];
        return base.GetHealthBarForecastSegments(context);
    }
    
    public string GetSecondAmount() => (DynamicVars["Stun"].BaseValue * (decimal) Math.Pow(2, count)).ToString();
}