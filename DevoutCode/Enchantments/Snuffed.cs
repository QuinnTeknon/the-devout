using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Enchantments;

public class Snuffed : DevoutEnchantment {
    public override bool CanEnchant(CardModel card) => card.Enchantment is Soulflame;

    protected override void OnEnchant() {
        Card.AddKeyword(CardKeyword.Ethereal);
        Card.AddKeyword(CardKeyword.Unplayable);
    }
}