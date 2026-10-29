using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Tokens;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers.Debuffs;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Flagellance;

public class FlagellantPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public readonly SpireResource piety;
    public readonly SpireResource penance;

    private bool active;

    public FlagellantPower() {
        piety = new("piety", (new Vector2(0, -64), 180, -1));
        penance = new("penance", (new Vector2(0, 64), -45, -1));
        //MainFile.Logger.Info($"Piety: {piety?.counter ?? null}, Penance: {penance?.counter ?? null}");
    }

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta) {
        if (active) {
            if (creature.HasPower<LifestealPower>()) {
                await creature.GetPower<LifestealPower>()?.Activate();
            } else {
                PlayerCombatState? player = creature.Player?.PlayerCombatState ?? null;
                if (player != null) {
                    if (!player.Hand.Cards.Any(card => card is WhisperingShade)) {
                        switch (Math.Sign(delta)) {
                            case +1: await piety.Gain(Amount, info: delta); break;
                            case -1: await penance.Gain(Amount, info: Math.Abs(delta)); break;
                        }
                    }
                    Flash();
                }
            }
        }
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource) {
        piety.SetPlayer(Owner);
        penance.SetPlayer(Owner);
        piety.Set(0);
        penance.Set(0);
        return base.AfterApplied(applier, cardSource);
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState) {
        active = (side == CombatSide.Player);
        return base.BeforeSideTurnStart(choiceContext, side, participants, combatState);
    }
}