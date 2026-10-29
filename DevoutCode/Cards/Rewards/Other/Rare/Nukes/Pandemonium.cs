using BaseLib.Utils;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Debuffs;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Nukes;

[Pool(typeof(TokenCardPool))]
public class Pandemonium() : DevoutCard(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, (0, X)), IFlipCard<Apocalypse> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new LifestealVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<LifestealPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        Random RNG = new();
        List<Creature> list = Enumerable.Repeat(0, penance.CapturedXValue).SelectMany(c => CombatState.HittableEnemies).ToList();
        MainFile.Logger.Info($"PANDEMONIUM: {String.Join(", ", list)} | {String.Join(", ", Enumerable.Repeat(0, penance.CapturedXValue))} | {String.Join(", ", CombatState.HittableEnemies)}");
        list.Shuffle();
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (Creature enemy in list) {
            switch (RNG.Next(2)) {
                case 0:
                    DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(enemy).WithAttackerAnim("Cast", 0f)
                        .WithHitVfxNode((Func<Creature, Node2D>) (t =>
                            NScratchVfx.Create(t, true).Define(Colors.Indigo, target: t)))
                        .Execute(context);
                    MainFile.Logger.Info($"PANDEMONIUM[{enemy}]: SLASH");
                    break;
                case 1:
                    DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(enemy).WithAttackerAnim("Cast", 0f)
                        .WithHitVfxNode((Func<Creature, Node2D>) (t => 
                            GD.Load<PackedScene>("res://scenes/vfx/vfx_bite.tscn").Instantiate<Node2D>().Define(Colors.Indigo, target: t)))
                        .Execute(context);
                    MainFile.Logger.Info($"PANDEMONIUM[{enemy}]: BITE");
                    break;
            }
            await Cmd.Wait(0.1f);
        }
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["Lifesteal"].UpgradeValueBy(1);
    }
}