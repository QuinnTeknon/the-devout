using Devout.DevoutCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class StrengthThroughPain() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, (0, X)) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, penance.CapturedXValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}