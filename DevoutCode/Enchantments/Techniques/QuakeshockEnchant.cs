using System.Resources;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class QuakeshockEnchant : TechniqueEnchant {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<WeakPower>(3), new PowerVar<VulnerablePower>(3), new("ShockDamage", 15)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<RingingPower>()];
    
    protected override async Task Play(PlayerChoiceContext context, CardPlay play) {
        GD.Load<PackedScene>("res://scenes/vfx/vfx_flying_slash.tscn").Instantiate<Node2D>().Define(Colors.DodgerBlue, target: Card.CombatState?.HittableEnemies.FirstOrDefault());
        await DamageCmd.Attack(DynamicVars["ShockDamage"].BaseValue).FromCard(Card, play)
            .TargetingAllOpponents(Card.CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(context);
        await PowerCmd.Apply<WeakPower>(context, Card.CombatState.HittableEnemies, DynamicVars.Weak.IntValue, Card.Owner.Creature, Card);
        await PowerCmd.Apply<VulnerablePower>(context, Card.CombatState.HittableEnemies, DynamicVars.Vulnerable.IntValue, Card.Owner.Creature, Card);
        await PowerCmd.Apply<RingingPower>(context, Card.Owner.Creature, 2, Card.Owner.Creature, Card);
    }
}