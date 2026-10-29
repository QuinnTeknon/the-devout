using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Relics.Reward.Uncommon;

public class SoulOfLightAndShadow : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Power", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CliodnaPower>(), HoverTipFactory.FromPower<CaoineagPower>()];

    public override Task AfterCliodnaEntered(CliodnaPower cliodna) => PowerCmd.Apply(new ThrowingPlayerChoiceContext(), cliodna.ancilliary, cliodna.Owner, DynamicVars["Power"].IntValue, null, null);
    public override Task AfterCaoineagEntered(CaoineagPower caoineag) => PowerCmd.Apply(new ThrowingPlayerChoiceContext(), caoineag.ancilliary, caoineag.Owner, DynamicVars["Power"].IntValue, null, null);
}