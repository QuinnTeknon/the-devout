using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class SacredSmiteEnchant() : TechniqueEnchant(10) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new("Extra", 5)];
    public override Decimal ScalingDamage() => DynamicVars["Extra"].BaseValue * Card.Owner.Creature.GetPower<FlagellantPower>()?.piety.Amount ?? 0;
}