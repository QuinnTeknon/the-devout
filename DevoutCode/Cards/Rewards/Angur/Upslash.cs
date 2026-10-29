using BaseLib.Utils;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

public class Upslash() : TechniqueAbstract(1, CardRarity.Common, ModelDb.Enchantment<UpslashEnchant>(), Article.VOWEL), IFlipCard<Downslam>;