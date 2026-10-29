using Devout.DevoutCode.Powers.Buffs.Performances.Dances;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Performance.Dance;

public class DanceOfDeath() : DanceAbstract<DanceOfDeathPower>(1, 1, TargetType.Self, rarity: CardRarity.Rare, entry: true) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];
}