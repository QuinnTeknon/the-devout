using Devout.DevoutCode.Cards.Rewards.Braid.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Braid;

public class BashingBraid() : BraidAbstract(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move), new("Threshold", 10m)];
    
    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e=> e.GetPowerAmount<BraidboundPower>() >= DynamicVars["Threshold"].BaseValue) ?? false;

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        BashingBraid braid = this;
        DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(braid, play).Targeting(play.Target).WithHitFx($"vfx/vfx_attack_blunt", "sfx/sfx_attack_blunt").Execute(context);
        if (play.Target.GetPowerAmount<BraidboundPower>() >= DynamicVars["Threshold"].BaseValue) {
            SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
            await Cmd.Wait(1);
            DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(braid, play).Targeting(play.Target).WithHitFx($"vfx/vfx_attack_blunt", "sfx/sfx_attack_blunt").Execute(context);
        }
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}