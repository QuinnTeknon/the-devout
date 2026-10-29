using BaseLib.Cards.Variables;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class SearingSmiteEnchant : TechniqueEnchant {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new DisplayVar<SearingSmiteEnchant>("Level", e => e.Level.ToString()), new("Extra", 10)];
    public int Level => Card.IsMutable ? Card.CurrentUpgradeLevel : 0;
    public override int DisplayAmount => Level;

    public override bool Condition() => (Card.Owner.PlayerCombatState?.DrawPile.Cards).Any(c => c.Enchantment is Soulflame);
    public override Decimal ScalingDamage() => Condition() ? DynamicVars["Extra"].BaseValue : 0;
    
    protected override void OnEnchant() => ((Angur) Card).DynamicVars["Heavy"].UpgradeValueBy(Level);
}