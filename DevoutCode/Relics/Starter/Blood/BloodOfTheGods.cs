using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace Devout.DevoutCode.Relics.Starter.Blood;

public class BloodOfTheGods : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Starter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RegenPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<RegenPower>(), HoverTipFactory.FromPower<FlagellantPower>()];
    
    public override Task BeforeCombatStart() {
        Task regen = PowerCmd.Apply<RegenPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["RegenPower"].BaseValue, Owner.Creature, null);
        return Task.CompletedTask;
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room) {
        if (room is CombatRoom) {
            Flash();
            await PowerCmd.Apply<FlagellantPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1M, Owner.Creature, null);
        }
    }

    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<MilkOfLife>();
}