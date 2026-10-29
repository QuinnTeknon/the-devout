using Devout.DevoutCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Dances;

public class GlaistigPower() : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource) {
        if (power == this) {
            foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards.Where(c => c.Keywords.Contains(KeywordExtensions.Rhythmic))) {
                card.BaseReplayCount += (int) amount;
            }
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card) {
        if (card.Owner == Owner.Player && card.DynamicVars.ContainsKey("Rhythm")) {
            card.BaseReplayCount += Amount;
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnchanted(PlayerChoiceContext context, CardModel card) {
        if (card.Owner == Owner.Player && card.Keywords.Contains(KeywordExtensions.Rhythmic))
            card.BaseReplayCount += Amount;
        return Task.CompletedTask;
    }
}