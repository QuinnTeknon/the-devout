using Devout.DevoutCode.Cards.Rewards.Bite.Templates;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Bite;

public class BasicBite() : BiteAbstract(CardRarity.Common, 4, 0) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new("Increase", 2m)];
    
    public override void RiderEffect(PlayerChoiceContext context, Creature target) {
        IEnumerable<BiteAbstract> enumerable = Owner.PlayerCombatState?.AllCards.OfType<BiteAbstract>() ?? [];
        foreach (BiteAbstract item in enumerable) {
            item.BuffFromBitePlay(DynamicVars["Increase"].BaseValue);
        }
    }
    
    protected override void OnUpgrade() {
        DynamicVars["Increase"].UpgradeValueBy(1M);
    }
}