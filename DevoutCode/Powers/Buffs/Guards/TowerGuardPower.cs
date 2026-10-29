using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Powers.Buffs.Guards;

public class TowerGuardPower : StatusGuard<FrailPower, PowerModel> {
    protected override async Task SecondaryEffect(PlayerChoiceContext context, CardPlay play, Angur angur, string key = "Secondary") {
        angur.DynamicVars["Heavy"].UpgradeValueBy((angur.strikes - 1) * SecondAmount);
    }
}