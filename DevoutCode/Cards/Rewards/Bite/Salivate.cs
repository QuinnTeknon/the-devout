using Devout.DevoutCode.Cards.Rewards.Bite.Templates;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Bite;

public class Salivate() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        IEnumerable<BiteAbstract> enumerable = Owner.PlayerCombatState?.AllCards.OfType<BiteAbstract>() ?? [];
        foreach (BiteAbstract item in enumerable) {
            await CardPileCmd.Add(item, PileType.Hand);
            if (IsUpgraded) {
                item.EnergyCost.AddThisTurnOrUntilPlayed(-DynamicVars.Energy.IntValue);
            }
        }
    }
}