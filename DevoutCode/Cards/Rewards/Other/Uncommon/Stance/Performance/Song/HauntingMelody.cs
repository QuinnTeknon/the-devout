using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Dances;
using Devout.DevoutCode.Powers.Buffs.Performances.Songs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.angur.powers;

public class HauntingMelody() : SongAbstract<HauntingMelodyPower>(2, 1, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.Static(StaticHoverTip.Block)];
}