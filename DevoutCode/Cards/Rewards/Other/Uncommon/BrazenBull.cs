using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class BrazenBull() : DevoutCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<PlatingPower>(), ..HoverTipFactory.FromEnchantment<Soulflame>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(4)];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<BrazenBullPower>(context, Owner.Creature, DynamicVars["PlatingPower"].IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() => DynamicVars["PlatingPower"].UpgradeValueBy(2);
}