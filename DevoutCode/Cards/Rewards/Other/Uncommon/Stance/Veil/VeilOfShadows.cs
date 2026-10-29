using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.HoverTips;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

public class VeilOfShadows() : VeilAbstract<VeilOfShadowsPower>(2, 1), IFlipCard<VeilOfScreams> {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<LifestealPower>()];
}