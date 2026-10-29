using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

public class SwiftSlash() : TechniqueAbstract(1, CardRarity.Uncommon, ModelDb.Enchantment<SwiftSlashEnchant>(), Article.CONSONANT) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [..base.CanonicalKeywords, CardKeyword.Exhaust];
}