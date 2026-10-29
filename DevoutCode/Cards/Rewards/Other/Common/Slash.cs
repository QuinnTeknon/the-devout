using Devout.DevoutCode.Cards.Starters;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class Slash() : DevoutCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10M, ValueProp.Move),
        new CardsVar(1),
        new("PutBack", 1m)
    ];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars["PutBack"].IntValue);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(context, Owner, prefs, null, this);
        await CardPileCmd.Add(cards, PileType.Draw, CardPilePosition.Top);
    }
    
    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}