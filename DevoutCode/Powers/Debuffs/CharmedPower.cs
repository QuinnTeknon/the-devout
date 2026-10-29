using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Forms;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Debuffs;

public class CharmedPower : DevoutPower {
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Applier")];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource) {
        ((StringVar) DynamicVars["Applier"]).StringValue = applier == null ? "" : PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, Applier.Player.NetId);
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? source, CardPlay? play) {
        if (dealer == Owner && Applier == target && props.IsPoweredAttack())
            return -Amount;
        return 0;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == Owner.Side)
            await PowerCmd.Remove(this);
    }
}