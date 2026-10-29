using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Dances;

public abstract class VeilPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private CardModel? Source;
    private CardModel? Dupe;

    public override Task AfterApplied(Creature? applier, CardModel? source) {
        Source = source;
        Dupe = Source.CreateDupe(source.Owner);
        return base.AfterApplied(applier, source);
    }
    
    public override async Task BeforeDamageReceived(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) {
        if (target == Owner && dealer != null && (props.IsPoweredAttack() || source is Omnislice)) {
            Flash();
            Activate(context, target, amount, props, dealer, source);
        }
    }
    
    public async Task Activate(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) { }
    public async Task Deactivate(PlayerChoiceContext context, CombatSide side, Creature enemy) { }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        MainFile.Logger.Info($"TRYING TO DISCARD A TEMPORARY POWER '{Dupe}' '{Dupe?.HasBeenRemovedFromState}'...");
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Dupe, PileType.Discard), 0);
        if (side == CombatSide.Enemy) {
            foreach (Creature enemy in participants) {
                Deactivate(context, side, enemy);
            }
            PowerCmd.Remove(this);
        }
    }

    public int GetThornsModifier(Creature target) {
        return GeneralCmd.GetDualPowerModifier<ThornsPower, CaoineagThornsPower>(target, 5);
    }
}