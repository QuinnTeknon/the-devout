using BaseLib.Utils;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Aura;

[Pool(typeof(TokenCardPool))]
public class Domicile : Aura<Aegis> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(1), new DamageVar(6, ValueProp.Unpowered)];
    public override async Task Effect(PlayerChoiceContext context) {
        foreach (Creature enemy in CombatState.HittableEnemies) {
            NFireBurstVfx child = NFireBurstVfx.Create(enemy, 0.75f);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        }
        await CreatureCmd.Damage(context, CombatState.HittableEnemies, DynamicVars.Damage, Owner.Creature, this, null);
    }
    
    public override async Task HP(PlayerChoiceContext context) => await GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, this, null);
    public override async Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) {
        if (CheckResource("penance", res, gainer))
            await Effect(new ThrowingPlayerChoiceContext());
    }
}