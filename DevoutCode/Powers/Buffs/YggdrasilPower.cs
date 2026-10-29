using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Devout.DevoutCode.Powers.Buffs;

public class YggdrasilPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private int delayed;

    public override async Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) {
        if (CheckResource("piety", res, gainer)) {
            await GeneralCmd.Lifesteal(new ThrowingPlayerChoiceContext(), null, gainer.Creature, -(info?.ToString()?.ToInt() ?? 0), null);
            if (new[]{PlayerTurnPhase.End, PlayerTurnPhase.AutoPostPlay}.Contains(gainer.PlayerCombatState.Phase))
                delayed++;
            else
                await Effect(gainer);
        }
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState state) {
        if (side == CombatSide.Player)
            for (int d = 0; d < delayed; d++)
                await Effect(Owner.Player);
    }

    public async Task Effect(Player gainer) {
        Flash();
        await PlayerCmd.GainEnergy(Amount, gainer);
        await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), Amount, gainer);
    }
}