using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

public class SacredSmite() : TechniqueAbstract(1, CardRarity.Uncommon, ModelDb.Enchantment<SacredSmiteEnchant>(), Article.CONSONANT, true, (2, 0)), IFlipCard<ShadowedSmite>;