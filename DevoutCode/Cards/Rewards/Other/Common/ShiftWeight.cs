using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class ShiftWeight() : DevoutCard(0, CardType.Skill, CardRarity.Common, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2M, ValueProp.Move), new("Buff", 1), new("Counter", 0)];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (IsUpgraded) {
            PowerCmd.Apply<ShiftStrengthPower>(context, Owner.Creature, DynamicVars["Buff"].BaseValue, Owner.Creature, this);
            PowerCmd.Apply<ShiftDexterityPower>(context, Owner.Creature, DynamicVars["Buff"].BaseValue, Owner.Creature, this);
        }
    }

    public override async Task AfterAngurUnsheathed(PlayerChoiceContext context, Tokens.Angurvadal.Angur angur) {
        if (angur.Owner == Owner && Pile is { Type: PileType.Discard }) {
            await CardPileCmd.Add(this, PileType.Hand);
        }
    }
}