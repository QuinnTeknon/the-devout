using Devout.DevoutCode.Cards.Rewards.Braid.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Braid;

public class CoilingBraid() : BraidAbstract(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("ConstrictPower", 3m)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        CoilingBraid braid = this;
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
        PowerCmd.Apply<BraidboundPower>(context, play.Target, DynamicVars["ConstrictPower"].IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() {
        DynamicVars["ConstrictPower"].UpgradeValueBy(2m);
    }
}