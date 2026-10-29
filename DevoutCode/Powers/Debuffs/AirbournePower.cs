using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Debuffs;

public class AirbournePower : DevoutPower, IHasSecondAmount {
    private const string _damageDecreaseKey = "DamageDecrease";
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("DamageDecrease", 50m), new DisplayVar<AirbournePower>("LandingDamage", p => (p.Force * p.Amount).ToString())];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, StunIntent.GetStaticHoverTip()];
    public int Force;
    
    public string GetSecondAmount() => Force.ToString();
    
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? source, CardPlay? play) {
        if (!(target == Owner || dealer == Owner))
            return 1m;
        if (!props.IsPoweredAttack())
            return 1m;
        return DynamicVars["DamageDecrease"].BaseValue / 100m;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext context, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource) {
        if (target == Owner && cardSource != null)
            return PowerCmd.ModifyAmount(context, this, 1, dealer, cardSource);
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) => PowerCmd.Remove(this);

    public override Task AfterRemoved(Creature owner) => CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner, Force * Amount, ValueProp.Unpowered, Owner, null, null);
}