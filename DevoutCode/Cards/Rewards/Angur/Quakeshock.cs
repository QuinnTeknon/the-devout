using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

public class Quakeshock() : TechniqueAbstract(2, CardRarity.Rare, ModelDb.Enchantment<QuakeshockEnchant>(), Article.CONSONANT) { }