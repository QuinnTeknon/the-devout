using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Debuffs;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class TidalSlashEnchant() : TechniqueEnchant(5) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<CharmedPower>(2), new RepeatVar(2), new("Tide", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>(), HoverTipFactory.FromPower<CharmedPower>()];

    public override bool Condition() => Card.Owner.Creature.HasPower<CaoineagPower>();
    
    protected override async Task Play(PlayerChoiceContext context, CardPlay play) {
        Angur angur = (Angur) Card;
        await CreatureCmd.TriggerAnim(Card.Owner.Creature, "Cast", Card.Owner.Character.CastAnimDelay);
        List<Creature> l = (Card.CombatState?.HittableEnemies ?? []).ToList();
        NCombatRoom instance = NCombatRoom.Instance;
        for (int i = 0; i < Convert.ToInt16(Condition()) + 1; i++) {
            if (i == 1) {
                l.Reverse();
            }
            foreach (Creature enemy in l) {
                NGoopyImpactVfx splash = (NGoopyImpactVfx) NGoopyImpactVfx.Create(enemy);
                splash.Modulate = Colors.RoyalBlue;
                instance?.CombatVfxContainer.AddChildSafely(splash);
                for (int j = 0; j < DynamicVars.Repeat.IntValue; j++) {
                    PowerCmd.Apply<CharmedPower>(context, enemy, DynamicVars["CharmedPower"].IntValue, Card.Owner.Creature, play.Card);
                    CreatureCmd.Damage(context, enemy, enemy.GetPowerAmount<CharmedPower>() + angur.vigor, ValueProp.Unpowered, Card, play);
                    await Cmd.Wait(0.05f);
                    if (Card.CombatState?.HittableEnemies.All(e => e.IsDead) ?? true) return;
                }
                await Cmd.Wait(0.1f);
            }
            await Cmd.Wait(0.5f);
        }
    }
}