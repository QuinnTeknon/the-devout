using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs;

public class HearthlightPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy) {
        if (!(card.IsUpgraded && card.Owner == Owner.Player))
            return;
        await CreatureCmd.Heal(Owner, Amount);
        await PowerCmd.Apply<HearthStrengthPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
    }
}