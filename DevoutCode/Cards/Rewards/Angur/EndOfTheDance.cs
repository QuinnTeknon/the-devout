using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments.Techniques;
using Devout.DevoutCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

public class EndOfTheDance() : TechniqueAbstract(3, CardRarity.Uncommon, ModelDb.Enchantment<EndOfTheDanceEnchant>(), Article.DEFINITE, newline: true) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, Hovertip("COUP_DE_GRACE")];

    public override Task AfterCardEnteredCombat(CardModel card) {
        if (card != this || IsClone)
            return Task.CompletedTask;
        int amount = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.CardPlay.Card is Tokens.Angurvadal.Angur && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
        GeneralCmd.ReduceCost(this, amount);
        return Task.CompletedTask;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay) {
        if (!(cardPlay.Card is Tokens.Angurvadal.Angur && cardPlay.Card.Owner == Owner))
            return Task.CompletedTask;
        GeneralCmd.ReduceCost(this, 1);
        return Task.CompletedTask;
    }
}