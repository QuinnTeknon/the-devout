using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers.Buffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class KinderDays() : DevoutCard(3, CardType.Power, CardRarity.Rare, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<KinderDaysPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, ..HoverTipFactory.FromEnchantment<Soulflame>(), ..HoverTipFactory.FromEnchantment<Snuffed>()];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<KinderDaysPower>(context, Owner.Creature, DynamicVars["KinderDaysPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() {
        
    }
}