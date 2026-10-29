using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Powers.Buffs;

public class HeartAndSoulPower() : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CliodnaPower>(), HoverTipFactory.FromPower<CaoineagPower>()];

    public override Task AfterCliodnaEntered(CliodnaPower cliodna) => PowerCmd.Apply(new ThrowingPlayerChoiceContext(), cliodna.ancilliary, cliodna.Owner, Amount, null, null);
    public override Task AfterCaoineagEntered(CaoineagPower caoineag) => PowerCmd.Apply(new ThrowingPlayerChoiceContext(), caoineag.ancilliary, caoineag.Owner, Amount, null, null);
}