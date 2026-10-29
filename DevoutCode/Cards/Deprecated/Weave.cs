using BaseLib.Utils;
using Devout.DevoutCode.Cards.Rewards.Braid.Templates;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Deprecated;

[Pool(typeof(TokenCardPool))]
public class Weave() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        IEnumerable<BraidAbstract> enumerable = Owner.PlayerCombatState?.AllCards.OfType<BraidAbstract>() ?? [];
        foreach (BraidAbstract item in enumerable) {
            await CardPileCmd.Add(item, PileType.Hand);
            if (IsUpgraded) {
                item.EnergyCost.AddThisTurnOrUntilPlayed(-DynamicVars.Energy.IntValue);
            }
        }
    }
}