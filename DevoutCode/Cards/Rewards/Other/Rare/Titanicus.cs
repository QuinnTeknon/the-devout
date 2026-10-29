using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class Titanicus() : DevoutCard(3, CardType.Power, CardRarity.Rare, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TitanicusPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<StrengthPower>()];

	public override async Task Play(PlayerChoiceContext context, CardPlay play) {
		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<TitanicusPower>(context, Owner.Creature, DynamicVars["TitanicusPower"].BaseValue, Owner.Creature, this);
	}

	protected override void OnUpgrade() => DynamicVars["TitanicusPower"].UpgradeValueBy(1);
}