using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Angurvadal;

[Pool(typeof(TokenCardPool))]
public class Angurvadal : Angur {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HeavyVar(4), 
        new DamageVar(15, ValueProp.Move),
        ..MakeCalculatedDamage("Piety", 0,(card, _) => card.Owner.Creature.GetPower<FlagellantPower>().piety.Amount),
        ..MakeCalculatedDamage("Penance", 0,(card, _) => card.Owner.Creature.GetPower<FlagellantPower>().penance.Amount),
        new DisplayVar<Angur>("TrueDamage", card=> $"{(card.CombatState != null ? (card.Enchantment != null ? (card.Owner.GetRelic<MysticLighter>() != null ? $"[blue]{DamageLoc(card)}[/blue]" : $"[purple]{DamageLoc(card)}[/purple]") : (DamageLoc(card) < card.DynamicVars.Damage.BaseValue ? $"[red]{DamageLoc(card)}[/red]" : (DamageLoc(card) > card.DynamicVars.Damage.BaseValue ? $"[green]{DamageLoc(card)}[/green]" : DamageLoc(card)))) : card.DynamicVars.Damage.BaseValue)}")
    ];

    public override CardRarity Rarity => CardRarity.Ancient;

    public override decimal BaseDamage() =>
            DynamicVars.Damage.IntValue +
            ((CalculatedDamageVar) DynamicVars["Piety"]).Calculate(Owner.Creature) +
            ((CalculatedDamageVar) DynamicVars["Penance"]).Calculate(Owner.Creature);
}