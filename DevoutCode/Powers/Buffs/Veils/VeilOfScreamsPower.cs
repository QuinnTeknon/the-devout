using Devout.DevoutCode.Powers.Buffs.Dances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

public class VeilOfScreamsPower : VeilPower {
    public async Task Activate(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) {
        await PowerCmd.Apply<CaoineagThornsPower>(context, target, Amount, target, null);
    }
}