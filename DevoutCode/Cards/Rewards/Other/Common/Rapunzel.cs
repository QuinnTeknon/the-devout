using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class Rapunzel() : UnsheatheCard(1, CardType.Power, CardRarity.Basic, TargetType.Self, upgradable: false) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new IntVar("Threshold", 10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<BraidboundPower>(), Hovertip("HEAVY")];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        
    }

    //protected override void OnUpgrade() => DynamicVars["Threshold"].UpgradeValueBy(-3);
}