using Devout.DevoutCode.Powers.Buffs.Performances.Dances;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Performance.Dance;

public class BindingDance() : DanceAbstract<BindingDancePower>(2, 1, TargetType.AnyEnemy) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<BraidboundPower>()];
}