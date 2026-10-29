using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Debuffs;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Ancients;

public class ShadowEmbrace() : DevoutCard(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, (0, 2)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8M, ValueProp.Move),
        new RepeatVar(3),
        new LifestealVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<LifestealPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        ShadowEmbrace card = this;
        ArgumentNullException.ThrowIfNull(play.Target);
        
        //VfxCmd.PlayOnCreature(card.Owner.Creature, VfxCmd.spookyScreamVfx);
        
        await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard(card, play).Targeting(play.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NScratchVfx.Create(t, true))).Execute(context);
        await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard(card, play).Targeting(play.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NScratchVfx.Create(t, true))).Execute(context);
        await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard(card, play).Targeting(play.Target).WithHitFx($"vfx/vfx_bite").Execute(context);

        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++) {
            await GeneralCmd.Lifesteal(context, play.Target, Owner.Creature, DynamicVars["Lifesteal"].BaseValue, this);
        }
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars["Lifesteal"].UpgradeValueBy(1M);
    }
}