using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Heart;

public class SacredHeart() : DevoutCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self), IFlipCard<HeartOfDarkness> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(7), new EnergyVar(1), new CardsVar(1)];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade() {
        AddKeyword(CardKeyword.Retain);
    }
}