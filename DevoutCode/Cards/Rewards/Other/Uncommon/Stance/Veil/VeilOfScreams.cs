using BaseLib.Utils;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

[Pool(typeof(TokenCardPool))]
public class VeilOfScreams() : VeilAbstract<VeilOfScreamsPower>(1, 1), IFlipCard<VeilOfSong> {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<ThornsPower>()];
}