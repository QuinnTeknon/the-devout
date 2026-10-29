using Devout.DevoutCode.Powers.Buffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class AngelEngine() : DevoutCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AngelEnginePower>(1m), new EnergyVar(1)];

	public override async Task Play(PlayerChoiceContext context, CardPlay play) {
		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<AngelEnginePower>(context, Owner.Creature, DynamicVars["AngelEnginePower"].BaseValue, Owner.Creature, this);
	}

	protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}