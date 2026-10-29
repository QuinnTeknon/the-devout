using BaseLib.Cards.Variables;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Bite.Templates;

public abstract class BiteCharmed(int basic, int upgrade, CardRarity rarity, int damage, int cost, (int, int) flagellant = default) : BiteAbstract(rarity, damage, cost, flagellant) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new("Rider", basic), new DisplayVar<BiteCharmed>("RiderPlus", model => $"{(model.IsUpgraded ? "[green]" : "")}{model.DynamicVars["Rider"].IntValue + upgrade}{(model.IsUpgraded ? "[/green]" : "")}")];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CharmedPower>()];

    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e=> e.HasPower<CharmedPower>()) ?? false;
    
    public decimal Value(Creature target) => DynamicVars["Rider"].IntValue + Convert.ToInt16(IsCharmed(target)) * upgrade;
    public bool IsCharmed(Creature target) => target.HasPower<CharmedPower>();
    
    protected override void OnUpgrade() {
        DynamicVars["Rider"].UpgradeValueBy(1M);
    }
}