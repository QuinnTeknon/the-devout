using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;

public class CaoineagThornsPower : StanceAncilliaryPower<ThornsPower> {
    public override async Task BeforeDamageReceived(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) {
        if (target == Owner && dealer != null && (props.IsPoweredAttack() || source is Omnislice)) {
            Flash();
            await CreatureCmd.Damage(context, dealer, Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, source, null);
        }
    }
}