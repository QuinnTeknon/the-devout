using BaseLib.Utils;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Rewards.Angur;

[Pool((typeof(TokenCardPool)))]
public class SearingSmite() : TechniqueAbstract(1, CardRarity.Rare, ModelDb.Enchantment<SearingSmiteEnchant>(), Article.CONSONANT, true) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, Hovertip("HEAVY")];
    
    protected override bool upgraded => false;
    public override int MaxUpgradeLevel => 666;
}