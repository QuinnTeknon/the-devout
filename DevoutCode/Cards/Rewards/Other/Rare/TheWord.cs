using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class TheWord() : DevoutCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self, (7, 0)) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(7)];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.Heal.IntValue);
    }

    protected override void OnUpgrade() {
        AddKeyword(CardKeyword.Retain);
    }
}