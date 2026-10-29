using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Dances;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

public class VeilOfShadowsPower : VeilPower {
    public async Task Activate(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) {
        await GeneralCmd.Lifesteal(context, dealer, target, Amount, null);
    }
}