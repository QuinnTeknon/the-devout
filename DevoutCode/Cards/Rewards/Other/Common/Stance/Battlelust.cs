using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common.Stance;

public class Battlelust() : DevoutCard(0, CardType.Skill, CardRarity.Common, TargetType.Self, (1, 0)) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CliodnaPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) => StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CLIODNA, this);

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}