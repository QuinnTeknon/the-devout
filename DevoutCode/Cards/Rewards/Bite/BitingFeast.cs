using Devout.DevoutCode.Cards.Rewards.Bite.Templates;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Bite;

public class BitingFeast() : BiteCharmed(1, 1, CardRarity.Uncommon, 4, 2, (0, 2)) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<LifestealPower>()];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        IEnumerable<BiteAbstract> enumerable = Owner.PlayerCombatState?.AllCards.OfType<BiteAbstract>() ?? [];
        foreach (BiteAbstract item in enumerable) {
            base.Play(context, play);
        }
    }

    public override void RiderEffect(PlayerChoiceContext context, Creature target) {
        GeneralCmd.Lifesteal(context, target, Owner.Creature, DynamicVars.CalculatedDamage.IntValue, this);
    }
    
    protected override void OnUpgrade() {
        base.OnUpgrade();
        DynamicVars.Damage.UpgradeValueBy(1M);
    }
}