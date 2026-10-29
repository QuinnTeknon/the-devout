using Devout.DevoutCode.Patches.Vars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Fatality() : DevoutCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15M, ValueProp.Move), new HeavyVar(2)];
    
    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e=> e.IsStunned) ?? false;

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue * (ShouldGlowGoldInternal ? 3 : 1)).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3").Execute(context);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5M);
        DynamicVars["Heavy"].UpgradeValueBy(1);
    }
}