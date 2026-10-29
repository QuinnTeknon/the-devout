using BaseLib.Utils;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

[Pool(typeof(TokenCardPool))]
public class VeilOfSong() : VeilAbstract<VeilOfSongPower>(1, 1), IFlipCard<VeilOfShadows> {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CharmedPower>()];
}