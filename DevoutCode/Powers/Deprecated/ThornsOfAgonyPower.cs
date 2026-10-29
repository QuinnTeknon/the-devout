using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Dances;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

public class ThornsOfAgonyPower : VeilPower {
    public async Task Activate(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) {
        await CreatureCmd.Damage(context, dealer, Amount + GeneralCmd.GetPowerModifier<BraidboundPower>(dealer, 5), ValueProp.Unpowered | ValueProp.Move, source, null);
    }
}