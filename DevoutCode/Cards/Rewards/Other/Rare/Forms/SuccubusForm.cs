using BaseLib.Utils;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Forms;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Forms;

[Pool(typeof(TokenCardPool))]
public class SuccubusForm() : DevoutCard(1, CardType.Power, CardRarity.Ancient, TargetType.Self, (0, 6)), IFlipCard<SeraphForm> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BraidboundPower>(1m), new LifestealVar(13)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<BraidboundPower>(), HoverTipFactory.FromPower<CharmedPower>(), HoverTipFactory.FromPower<LifestealPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<SuccubusFormPower>(context, Owner.Creature, DynamicVars["BraidboundPower"].BaseValue, Owner.Creature, this);
	}

	protected override void OnUpgrade() {
		DynamicVars["BraidboundPower"].UpgradeValueBy(1);
	}
}