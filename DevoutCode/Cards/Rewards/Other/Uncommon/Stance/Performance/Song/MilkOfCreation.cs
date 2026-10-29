using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers.Buffs.Dances;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace Devout.DevoutCode.Cards.angur.powers;

public class MilkOfCreation() : SongAbstract<MilkOfCreationPower>(1, 1, TargetType.Self, rarity: CardRarity.Rare, entry: true) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, ..HoverTipFactory.FromEnchantment<Soulflame>()];
}