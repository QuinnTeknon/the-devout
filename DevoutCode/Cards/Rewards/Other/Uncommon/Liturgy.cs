using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Liturgy() : DevoutCard(X, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(7), new HpLossVar(2), new RepeatVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    private bool active;
    
    protected override void AddExtraArgsToDescription(LocString description) {
        description.Add("IsActive", active);
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType pile, AbstractModel? clonedBy) {
        if (card is Liturgy)
            if (pile == PileType.Play) {
                active = true;
                card.BaseReplayCount -= ResolveEnergyXValue() + (IsUpgraded ? card.DynamicVars.Repeat.IntValue : 0);
            } else if (card.Pile.Type == PileType.Play) {
                active = false;
                card.BaseReplayCount += ResolveEnergyXValue() + (IsUpgraded ? card.DynamicVars.Repeat.IntValue : 0);
            }
    }

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, this, play);
    }
}