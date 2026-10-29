using Devout.DevoutCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Ancients;

public class Syzygy() : DevoutCard(X, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies, (X, X)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5M, ValueProp.Move),
        new RepeatVar(3),
        new HealVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override bool ShouldGlowGoldInternal => Alignment().Item2;

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        Syzygy card = this;
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(Alignment().Item1).FromCard(this, play)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_starry_impact")
            .Execute(context);
    }

    protected (int, bool) Alignment() {
        var energyLost = ResolveEnergyXValue();
        var finalCost = Math.Max(energyLost, Math.Max(piety.CapturedXValue, penance.CapturedXValue));
        return finalCost.Factorises([energyLost, piety.CapturedXValue, penance.CapturedXValue])
            ? (energyLost + piety.CapturedXValue + penance.CapturedXValue, true)
            : (finalCost, false);
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(1M);
    }
}