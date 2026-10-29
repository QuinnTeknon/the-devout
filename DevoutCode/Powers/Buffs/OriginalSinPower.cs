using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Cards.Tokens;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Devout.DevoutCode.Powers.Buffs;

public class OriginalSinPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    public override Task BeforeSideTurnStart(PlayerChoiceContext context, CombatSide side, IReadOnlyList<Creature> participants, ICombatState state) {
        if (side == CombatSide.Player) {
            CardPileCmd.AddToCombatAndPreview<WhisperingShade>(Owner, PileType.Hand, Amount, Owner.Player);
        }
        return base.BeforeSideTurnStart(context, side, participants, state);
    }
}