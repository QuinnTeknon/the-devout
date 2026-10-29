using Devout.DevoutCode.Powers.Buffs.Flagellance;
using Devout.DevoutCode.Powers.Debuffs;
using Devout.DevoutCode.Relics;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Relics.Reward.Rare;

public class ScythesOfPandora : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmedPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<FlagellantPower>(), HoverTipFactory.FromPower<CharmedPower>()];
}