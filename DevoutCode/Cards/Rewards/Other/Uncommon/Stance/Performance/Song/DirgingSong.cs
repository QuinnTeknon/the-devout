using Devout.DevoutCode.Powers.Buffs.Performances.Songs;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.angur.powers;

public class DirgingSong() : SongAbstract<DirgingSongPower>(1, 1, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CharmedPower>()];
}