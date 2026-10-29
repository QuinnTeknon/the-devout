using Devout.DevoutCode.Cards.Rewards.Braid.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Braid;

public class SqueezingBraid() : BraidAbstract(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(6m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((_, target) => target?.GetPowerAmount<BraidboundPower>() ?? 0)
    ];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        SqueezingBraid braid = this;
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
        await Cmd.Wait(1f);
        DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(braid, play).Targeting(play.Target).WithHitFx($"vfx/vfx_attack_blunt", "blunt_attack.mp3").Execute(context);
    }

    protected override void OnUpgrade() {
        DynamicVars.CalculationBase.UpgradeValueBy(3m);
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}