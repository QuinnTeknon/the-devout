using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Bite.Templates;

public abstract class BiteAbstract(CardRarity rarity, int damage, int cost, (int, int) flagellant = default) : DevoutCard(cost, CardType.Attack, rarity, TargetType.AnyEnemy, flagellant) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(damage, ValueProp.Move)];
    protected override HashSet<CardTag> CanonicalTags => [];
    
    public decimal ExtraDamageFromBitePlays = 0;

    public override Task Play(PlayerChoiceContext context, CardPlay play) {
        BiteAbstract card = this;
        return DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard(card, play).Targeting(play.Target).WithHitFx($"vfx/vfx_bite").Execute(context);
    }

    public abstract void RiderEffect(PlayerChoiceContext context, Creature target);
    
    public void BuffFromBitePlay(decimal damage) {
        DynamicVars.Damage.BaseValue += damage;
        ExtraDamageFromBitePlays += damage;
    }
    
    public override async Task AfterDamageGiven(PlayerChoiceContext context, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? source) {
        if (dealer == Owner.Creature && source == this && props.IsPoweredAttack() && result.UnblockedDamage >= RiderThreshold()) {
            RiderEffect(context, target);
        }
    }

    public int RiderThreshold() => 1;
}