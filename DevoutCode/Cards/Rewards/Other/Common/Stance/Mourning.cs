using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common.Stance;

public class Mourning() : DevoutCard(0, CardType.Skill, CardRarity.Common, TargetType.Self, (0, 1)) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) => StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CAOINEAG, this);

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}