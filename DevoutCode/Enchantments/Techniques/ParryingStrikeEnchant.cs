using BaseLib.Extensions;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class ParryingStrikeEnchant : TechniqueEnchant {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new BlockVar(10, ValueProp.Move), new PowerVar<ThornsPower>(5)];
    protected override async Task Play(PlayerChoiceContext context, CardPlay? play) {
        await CreatureCmd.GainBlock(Card.Owner.Creature, DynamicVars.Block, play);
        await PowerCmd.Apply<ParryingThornsPower>(context, Card.Owner.Creature, DynamicVars.Power<ThornsPower>().BaseValue, Card.Owner.Creature, Card);
    }
}