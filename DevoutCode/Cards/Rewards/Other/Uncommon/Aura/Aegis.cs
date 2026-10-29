using Devout.DevoutCode.Extensions.Resource;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Aura;

public class Aegis : Aura<Domicile> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1), new BlockVar(3, ValueProp.Unpowered)];
    public override async Task Effect(PlayerChoiceContext context) {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null, true);
    }

    public override async Task HP(PlayerChoiceContext context) => await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    public override async Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) {
        if (CheckResource("piety", res, gainer))
            await Effect(new ThrowingPlayerChoiceContext());
    }
}