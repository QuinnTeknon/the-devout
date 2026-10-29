using BaseLib.Utils;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

[Pool(typeof(TokenCardPool))]
public class ShadowedSmite() : TechniqueAbstract(1, CardRarity.Uncommon, ModelDb.Enchantment<ShadowedSmiteEnchant>(), Article.CONSONANT, true, (0, 2)), IFlipCard<SacredSmite>;