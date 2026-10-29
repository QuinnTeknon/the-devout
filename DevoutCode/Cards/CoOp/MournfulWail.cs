using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.CoOp;

public class MournfulWail() : DevoutCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2, ValueProp.Move), new MaxHpVar(0), new("Threshold", 10)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

    protected override bool ShouldGlowRedInternal => !IsInCorrectStance(StanceCmd.Stance.CAOINEAG);
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        for (int i = 0; i < 1 + (DynamicVars.MaxHp.BaseValue / DynamicVars["Threshold"].BaseValue); i++) {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(CombatState).Execute(context);
        }
    }

    public override async Task AfterDeath(PlayerChoiceContext context, Creature creature, bool wasRemovalPrevented, float deathAnimLength) {
        if (creature.Side == CombatSide.Player && Pile is { Type: PileType.Exhaust }) {
            if (DynamicVars.MaxHp.BaseValue >= DynamicVars["Threshold"].BaseValue) {
                DynamicVars.MaxHp.BaseValue = creature.MaxHp;
                await CardCmd.AutoPlay(context, this, null);
            }
        }
    }
    
    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}