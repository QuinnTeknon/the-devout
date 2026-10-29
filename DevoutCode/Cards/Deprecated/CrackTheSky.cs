using BaseLib.Utils;
using Devout.DevoutCode.Enchantments;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Deprecated;

[Pool(typeof(TokenCardPool))]
public class CrackTheSky() : DevoutCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy, (X, 0)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new("Numerator", 1),
        new("Denominator", 2),
        ..MakeCalculatedDamage("Beam", 5, Scaling)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, ..HoverTipFactory.FromEnchantment<Soulflame>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        for (int i = 0; i < piety.CapturedXValue; i++) {
            Creature? target = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (target != null) {
                Vector2 pos = NCombatRoom.Instance?.GetCreatureNode(target)?.GetBottomOfHitbox() ?? Vector2.Zero;
                NLargeMagicMissileVfx missile = NLargeMagicMissileVfx.Create(pos, Colors.Gold);
                missile.Rotation += Mathf.DegToRad(30);
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(missile);
                NKinPriestBeamVfx beam = (NKinPriestBeamVfx) GD.Load<PackedScene>("res://Devout/scenes/vfx/crack_the_sky/crack_the_sky_vfx.tscn").Instantiate();
                beam.Rotation -= Mathf.DegToRad(90);
                beam.Modulate = Colors.Gold;
                beam.GlobalPosition = new Vector2(pos.X, 500);
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(beam);
                for (int j = 0; j < 5; j++) {
                    Cmd.Wait(0.005f);
                    SfxCmd.Play("event:/sfx/characters/regent/regent_stardust");
                }
                await Cmd.Wait(0.025f);
                CreatureCmd.Damage(context, target, DynamicVars["Beam"].IntValue, ValueProp.Move, play.Card, play);
            }
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    private static decimal Scaling(CardModel model, Creature? creature) => 0;//GeneralCmd.GetEnchantedCards<Soulflame>(creature.Player, PileType.Exhaust).Count * model.DynamicVars["Numerator"].IntValue / model.DynamicVars["Denominator"].IntValue;
}