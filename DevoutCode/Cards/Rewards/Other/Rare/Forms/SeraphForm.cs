using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Forms;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Forms;

public class SeraphForm() : DevoutCard(1, CardType.Power, CardRarity.Ancient, TargetType.Self, (6, 0)), IFlipCard<SuccubusForm> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Wings", 4m), new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<AscendingPower>(), HoverTipFactory.FromPower<RingingPower>(), ..HoverTipFactory.FromEnchantment<Soulflame>()];

	public override async Task Play(PlayerChoiceContext context, CardPlay play) {
		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<SeraphFormPower>(context, Owner.Creature, DynamicVars["Wings"].BaseValue, Owner.Creature, this);
	}

	protected override void OnUpgrade() {
		DynamicVars["Wings"].UpgradeValueBy(2);
	}
}