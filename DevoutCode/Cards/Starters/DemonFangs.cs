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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Starters;

public class DemonFangs() : DevoutCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, (0, 2)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8M, ValueProp.Move),
        new RepeatVar(2),
        new LifestealVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<LifestealPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        //VfxCmd.PlayOnCreature(card.Owner.Creature, VfxCmd.spookyScreamVfx);
        int vigor = Owner.Creature.GetPowerAmount<VigorPower>();
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NScratchVfx.Create(t, true))).Execute(context);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue + vigor).FromCard(this, play).Targeting(play.Target).WithHitFx($"vfx/vfx_bite").Execute(context);

        await GeneralCmd.Lifesteal(context, play.Target, Owner.Creature, DynamicVars["Lifesteal"].BaseValue, this);
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars["Lifesteal"].UpgradeValueBy(1M);
    }
}