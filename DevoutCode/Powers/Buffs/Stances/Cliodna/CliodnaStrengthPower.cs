using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;

public class CliodnaStrengthPower : StanceAncilliaryPower<StrengthPower> {
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? play) {
        if (Owner != dealer) {
            return 0m;
        }
        if (!props.IsPoweredAttack()) {
            return 0m;
        }
        return Amount;
    }
}