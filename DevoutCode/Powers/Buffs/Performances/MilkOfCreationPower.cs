using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Powers.Buffs.Performances;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Devout.DevoutCode.Powers.Buffs.Dances;

public class MilkOfCreationPower : RhythmicPower {
    public override async Task Activate(PlayerChoiceContext context, CardPlay play) {
        GD.Load<PackedScene>("res://scenes/vfx/vfx_bloody_impact.tscn").Instantiate<Node2D>().Define(Colors.White, 1, target: Owner);
        await Cmd.Wait(0.1f);
        await CreatureCmd.Heal(Owner, Amount);
    }
}