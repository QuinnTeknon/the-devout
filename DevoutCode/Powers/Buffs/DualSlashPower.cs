using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs;

public class DualSlashPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCardEnteredCombat(CardModel card) {
        if (card.Owner == Owner.Player && card is Angur) {
            card.BaseReplayCount += Amount;
            PowerCmd.Remove(this);
        }
        return Task.CompletedTask;
    }
}