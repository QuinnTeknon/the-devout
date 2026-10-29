using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Devout.DevoutCode.Cards.Starters;

public class PrayDevout() : DevoutCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(5), new HpLossVar(3), new CardsVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard<Burn>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, this, play);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade() => BaseReplayCount += 1;
}