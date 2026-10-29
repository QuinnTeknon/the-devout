using BaseLib.Abstracts;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Dances;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments;

public class Soulflame : DevoutEnchantment {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new HpLossVar(3)];
    
    public override bool CanEnchant(CardModel card) => true;

    protected override void OnEnchant() {
        Card.AddKeyword(CardKeyword.Ethereal);
        if (!Card.CanonicalKeywords.Contains(CardKeyword.Unplayable)) {
            Card.EnergyCost.UpgradeBy(-DynamicVars.Energy.IntValue);
        }
    }

    public async override Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        if (!Card.Owner.Creature.HasPower<MilkOfCreationPower>()) {
            
            await GeneralCmd.LoseHP(context, Card.Owner.Creature, DynamicVars.HpLoss.IntValue, Card, play);
        }
    }
}