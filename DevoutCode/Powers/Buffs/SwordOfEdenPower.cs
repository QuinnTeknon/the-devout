using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Devout.DevoutCode.Powers.Buffs;

public class SwordOfEdenPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Upgraded", 0)];

    public override Task AfterApplied(Creature? applier, CardModel? source) {
        DynamicVars["Upgraded"].BaseValue = Convert.ToDecimal(source.IsUpgraded);
        return base.AfterApplied(applier, source);
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext context, CombatSide side, IReadOnlyList<Creature> participants, ICombatState state) {
        if (!Convert.ToBoolean(DynamicVars["Upgraded"]) && side == CombatSide.Player) {
            CardPileCmd.AddToCombatAndPreview<Burn>(Owner, PileType.Hand, Amount, Owner.Player);
        }
        return base.BeforeSideTurnStart(context, side, participants, state);
    }
}