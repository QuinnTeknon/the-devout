using Devout.DevoutCode.Cards.Rewards.Bite.Templates;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Bite;

public class Devour() : DevoutCard(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, (0, 2)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1),
        new CalculationBaseVar(8M),
        new ExtraDamageVar(1M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => (card.Owner.PlayerCombatState?.AllCards.OfType<BiteAbstract>() ?? []).Sum(@abstract => @abstract.ExtraDamageFromBitePlays))];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    public override Task Play(PlayerChoiceContext context, CardPlay play) {
        Task<AttackCommand> attack = DamageCmd.Attack(DynamicVars.CalculatedDamage.IntValue).FromCard(this, play).Targeting(play.Target).WithHitFx($"vfx/vfx_bite").Execute(context);
        if (!Enumerable.Any(Enumerable.SelectMany(attack.Result.Results, r => r), (Func<DamageResult, bool>) (r => r.WasTargetKilled)))
            return Task.CompletedTask;
        return PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
    
    protected override void OnUpgrade() {
        DynamicVars.CalculationBase.UpgradeValueBy(2M);
        DynamicVars.Energy.UpgradeValueBy(1M);
    }
}