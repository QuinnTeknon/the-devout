using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Powers.Buffs.Performances;

public abstract class RhythmicPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayedLate(PlayerChoiceContext context, CardPlay play) {
        if (play.Card.Owner == Owner.Player) {
            Flash();
            Animation();
            await Activate(new ThrowingPlayerChoiceContext(), play);
        }
    }

    public virtual async Task Activate(PlayerChoiceContext context, CardPlay play) { }
    public virtual async Task Deactivate(PlayerChoiceContext context, CombatSide side, Creature enemy) { }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == CombatSide.Player)
            PowerCmd.Remove(this);
        else
            foreach (Creature enemy in participants)
                Deactivate(context, side, enemy);
    }

    public int GetDexterityModifier(Creature target) => GeneralCmd.GetPowerModifier<DexterityPower>(target, 5);

    public virtual void Animation() { }
}