using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Guards;

public abstract class StatusGuard<S1, S2> : GuardPower where S1 : PowerModel where S2 : PowerModel {
    public override async Task AfterAngurPlayed(PlayerChoiceContext context, CardPlay play, Angur angur) {
        switch (angur.strikes) {
            case 1:   await ApplyStatus<S1>(context, play, angur, "Primary"); break;
            case >1:  await SecondaryEffect(context, play, angur); break;
        }
    }

    private async Task ApplyStatus<S>(PlayerChoiceContext context, CardPlay play, Angur angur, string key) where S : PowerModel {
        await PowerCmd.Apply<S>(context, ModelDb.Power<S>().Type == PowerType.Debuff ? play.Target : angur.Owner.Creature, DynamicVars[key].BaseValue, angur.Owner.Creature, angur);
    }

    protected virtual async Task SecondaryEffect(PlayerChoiceContext context, CardPlay play, Angur angur, string key = "Secondary") {
        await ApplyStatus<S2>(context, play, angur, key);
    }
}