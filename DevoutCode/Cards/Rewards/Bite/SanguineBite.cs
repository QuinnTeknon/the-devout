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

public class SanguineBite() : BiteCharmed(2, 1, CardRarity.Common, 8, 1, (0, 1)) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<LifestealPower>()];

    public override void RiderEffect(PlayerChoiceContext context, Creature target) {
        GeneralCmd.Lifesteal(context, target, Owner.Creature, Value(target), this);
    }
    
    protected override void OnUpgrade() {
        base.OnUpgrade();
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars["Rider"].UpgradeValueBy(1M);
    }
}