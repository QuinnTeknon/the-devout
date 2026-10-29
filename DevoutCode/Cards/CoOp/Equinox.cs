using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Enchantments.Techniques;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.CoOp;

public class Equinox() : TechniqueAbstract(1, CardRarity.Rare, ModelDb.Enchantment<EquinoxEnchant>(), Article.NONE, true, (6, 6)) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, Hovertip("HEAVY")];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
}