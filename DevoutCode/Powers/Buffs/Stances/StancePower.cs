using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Stances;

public abstract class StancePower<T> : DevoutPower where T : PowerModel {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public T? ancilliary;

    public readonly int magnitude = 3;

    public override async Task AfterApplied(Creature? creature, CardModel? source) {
        await PowerCmd.Apply<T>(new ThrowingPlayerChoiceContext(), creature, magnitude, creature, null);
        ancilliary = creature.GetPower<T>();
    }

    public override Task AfterRemoved(Creature creature) {
        PowerCmd.Remove<T>(creature);
        return base.AfterRemoved(creature);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants) {
        if (side == CombatSide.Enemy) {
            await PowerCmd.Decrement(ancilliary);
            if (!Owner.HasPower<T>()) {
                PowerCmd.Remove(this);
            }
        }
    }
    
    public Task AfterEntered() {
        foreach (AbstractModel model in Owner.CombatState.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                switch (this) {
                   case CliodnaPower cliodna: 
                        switch (model) {
                            case DevoutCard card:
                                card.AfterCliodnaEntered(cliodna); break;
                            case DevoutPower power:
                                power.AfterCliodnaEntered(cliodna); break;
                            case DevoutRelic relic:
                                relic.AfterCliodnaEntered(cliodna); break;
                        }; break;
                   case CaoineagPower caoineag: 
                       switch (model) {
                           case DevoutCard card:
                               card.AfterCaoineagEntered(caoineag); break;
                           case DevoutPower power:
                               power.AfterCaoineagEntered(caoineag); break;
                           case DevoutRelic relic:
                               relic.AfterCaoineagEntered(caoineag); break;
                       }; break;
                }
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }
}