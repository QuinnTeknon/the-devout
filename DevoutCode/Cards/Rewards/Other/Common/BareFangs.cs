using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class BareFangs() : DevoutCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, (1, 0)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(1),
        new PowerVar<CharmedPower>(2)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<CharmedPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_bite");
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_spooky_scream");
        VfxCmd.PlayOnCreatureCenter(play.Target, "vfx/vfx_gaze");
        PowerCmd.Apply<CharmedPower>(context, play.Target, DynamicVars["CharmedPower"].IntValue, Owner.Creature, play.Card);
        PowerCmd.Apply<VulnerablePower>(context, play.Target, DynamicVars.Vulnerable.IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() => DynamicVars["CharmedPower"].UpgradeValueBy(1);
}