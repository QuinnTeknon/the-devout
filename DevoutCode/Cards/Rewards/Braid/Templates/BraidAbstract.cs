using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace Devout.DevoutCode.Cards.Rewards.Braid.Templates;

public abstract class BraidAbstract(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    (int, int) flagellant = default) : DevoutCard(cost, type, rarity, target, flagellant)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<BraidboundPower>()];
}