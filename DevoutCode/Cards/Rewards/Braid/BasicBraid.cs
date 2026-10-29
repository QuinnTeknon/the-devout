using Devout.DevoutCode.Cards.Rewards.Braid.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Braid;

public class BasicBraid() : BraidAbstract(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new("ConstrictPower", 1m)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        BasicBraid braid = this;
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(braid, play).Targeting(play.Target).WithHitFx($"vfx/vfx_attack_blunt").Execute(context);
        PowerCmd.Apply<BraidboundPower>(context, play.Target, DynamicVars["ConstrictPower"].IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["ConstrictPower"].UpgradeValueBy(1m);
    }
}