using BaseLib.Extensions;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Bullstomp() : DevoutCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(30, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        AttackCommand stomp = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt").Execute(context);
        DamageResult result = stomp.Results.SelectMany(r => r).First();
        if (!stomp.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
            return;
        GD.Load<PackedScene>("res://scenes/vfx/vfx_flying_slash.tscn").Instantiate<Node2D>().Define(Colors.White, whiteness: 1, rotation: 180, target: result.Receiver);
        GD.Load<PackedScene>("res://scenes/vfx/vfx_flying_slash.tscn").Instantiate<Node2D>().Define(Colors.White, whiteness: 1, target: result.Receiver);
        AttackCommand shockwave = await DamageCmd.Attack(result.OverkillDamage).FromCard(this, play).TargetingAllOpponents(CombatState).WithValueProp(ValueProp.Unpowered).WithHitVfxNode(NBigSlashImpactVfx.Create).Execute(context);
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(15);
    }
}