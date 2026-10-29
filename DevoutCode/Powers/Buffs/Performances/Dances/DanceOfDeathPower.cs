using Devout.DevoutCode.Powers.Buffs.Dances.Dances;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Devout.DevoutCode.Powers.Buffs.Performances.Dances;

public class DanceOfDeathPower : DancePower {
    public override async Task Activate(PlayerChoiceContext context, CardPlay play) {
        await PowerCmd.Apply<DanceStrengthPower>(context, Owner, Amount + GetDexterityModifier(Owner), play.Card.Owner.Creature, play.Card);
        await PowerCmd.Apply<DanceDexterityPower>(context, Owner, Amount + GetDexterityModifier(Owner), play.Card.Owner.Creature, play.Card);
    }
}