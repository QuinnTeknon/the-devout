using Devout.DevoutCode.Enchantments.Techniques.Templates;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class SwiftSlashEnchant : TechniqueEnchant {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new CardsVar(2), new EnergyVar(2)];
    protected override async Task Play(PlayerChoiceContext context, CardPlay play) {
        PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Card.Owner);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Card.Owner);
    }
}