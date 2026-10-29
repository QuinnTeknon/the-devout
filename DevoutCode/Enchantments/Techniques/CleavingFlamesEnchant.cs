using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
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

public class CleavingFlamesEnchant() : TechniqueEnchant(5) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<StrengthPower>(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CliodnaPower>(), HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromCard<Burn>(), ..HoverTipFactory.FromEnchantment<Soulflame>()];

    public override bool Condition() => Card.Owner.Creature.HasPower<CliodnaPower>();
    
    protected override async Task Play(PlayerChoiceContext context, CardPlay play) {
        Angur angur = (Angur) Card;
        await CreatureCmd.TriggerAnim(Card.Owner.Creature, "Cast", Card.Owner.Character.CastAnimDelay);
        if (Condition()) {
            await PowerCmd.Apply<StrengthPower>(context, Card.Owner.Creature, DynamicVars["StrengthPower"].IntValue, Card.Owner.Creature, play.Card);
            await Cmd.Wait(0.25f);
        }
        var target = Card.CombatState?.HittableEnemies.FirstOrDefault();
        if (target != null)
            VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_flying_slash");
        NCombatRoom instance = NCombatRoom.Instance;

        int dmg = 0;
        List<CardModel> draw = Card.Owner.PlayerCombatState?.DrawPile.Cards.OfType<Burn>().Select(c => (CardModel) c).Union(Card.Owner.PlayerCombatState?.DrawPile.Cards.Where(c => c.Enchantment is Soulflame)).ToList();
        List<CardModel> discard = Card.Owner.PlayerCombatState?.DiscardPile.Cards.OfType<Burn>().Select(c => (CardModel) c).Union(Card.Owner.PlayerCombatState?.DrawPile.Cards.Where(c => c.Enchantment is Soulflame)).ToList();
        IEnumerable<CardModel> cards = draw.Union(discard);
        List<Creature> l = (Card.CombatState?.HittableEnemies ?? []).ToList();
        foreach (CardModel card in cards) {
            l.Reverse();
            foreach (Creature enemy in l) {
                instance?.CombatVfxContainer.AddChildSafely((Node) NGroundFireVfx.Create(enemy));
                instance?.CombatVfxContainer.AddChildSafely((NFireBurstVfx) NFireBurstVfx.Create(enemy, 1));
                if (card.Enchantment is Soulflame) {
                    CreatureCmd.Damage(context, enemy, card.Enchantment.Amount + Card.Owner.Creature.GetPowerAmount<StrengthPower>() / 5 - DynamicVars.Damage.IntValue + angur.vigor, ValueProp.Unpowered, Card, play);
                }
                if (card is Burn) {
                    CreatureCmd.Damage(context, enemy, card.DynamicVars.Damage.BaseValue + Card.Owner.Creature.GetPowerAmount<StrengthPower>() / 5 - DynamicVars.Damage.IntValue + angur.vigor, ValueProp.Unpowered, Card, play);
                }
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                if (Card.CombatState?.HittableEnemies.All(e => e.IsDead) ?? true) return;
                await Cmd.Wait(0.1f);
            }
            CardCmd.Exhaust(context, card, true, true);
        }
    }
}