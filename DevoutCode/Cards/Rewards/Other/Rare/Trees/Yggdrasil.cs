using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Trees;

public class Yggdrasil() : DevoutCard(2, CardType.Power, CardRarity.Rare, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        await PowerCmd.Apply<YggdrasilPower>(context, Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, this);
        if (IsUpgraded) {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        }
    }

    protected override void OnUpgrade() {
        DynamicVars.Energy.UpgradeValueBy(1);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}