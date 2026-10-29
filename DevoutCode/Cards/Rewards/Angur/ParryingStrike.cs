using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

public class ParryingStrike() : TechniqueAbstract(1, CardRarity.Uncommon, ModelDb.Enchantment<ParryingStrikeEnchant>(), Article.CONSONANT, newline: true, type: CardType.Skill) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new BlockVar(0m, ValueProp.Move)];
}