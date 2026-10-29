using Devout.DevoutCode.Cards.Starters;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class Ruminate() : DevoutCard(1, CardType.Skill, CardRarity.Common, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, ..HoverTipFactory.FromEnchantment<Soulflame>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromSimpleGrid(context, Owner.PlayerCombatState.DiscardPile.Cards, Owner, prefs);
        foreach (CardModel card in cards) {
            GeneralCmd.Enchant(ModelDb.Enchantment<Soulflame>(), card);
        }
        await CardPileCmd.Add(cards, PileType.Draw, CardPilePosition.Top);
    }
    
    protected override void OnUpgrade() {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}