using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class GreatslashEnchant() : TechniqueEnchant(10) {
    protected override void OnEnchant() {
        ((Angur) Card).DynamicVars["Heavy"].UpgradeValueBy(1);
    }
}