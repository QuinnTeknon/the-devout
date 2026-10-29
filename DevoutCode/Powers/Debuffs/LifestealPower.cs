using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Forms;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Devout.DevoutCode.Powers.Debuffs;

public class LifestealPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public async Task Activate() {
        Flash();
        //await Owner.GetPower<SuccubusFormPower>()?.Bind(Applier);
        await Cmd.Wait(0.25f);
        RemoveInternal();
    }

    public override Task BeforeSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        RemoveInternal();
        return Task.CompletedTask;
    }
}