using Devout.DevoutCode.Cards.Rewards.Bite.Templates;
using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Bite;

public class CrushingBite() : BiteCharmed(1, 1, CardRarity.Common, 8, 1) {
    public override void RiderEffect(PlayerChoiceContext context, Creature target) {
        PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, -DynamicVars.CalculatedDamage.IntValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() {
        base.OnUpgrade();
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}