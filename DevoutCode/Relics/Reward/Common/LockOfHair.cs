using Devout.DevoutCode.Powers.Debuffs;
using Devout.DevoutCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Relics.Reward.Common;

public class LockOfHair : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Common;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<BraidboundPower>()];

    public override async Task AfterBraidboundStun(BraidboundPower power) {
        Flash();
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue);
    }
}