using BaseLib.Cards.Variables;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Powers.Buffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class Gigantes() : UnsheatheCard(3, CardType.Power, CardRarity.Rare, TargetType.Self, upgradable: false) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<GigantesPower>(1), 
        new("DamageBase", 25), new("DamageAddition", 25),
        new DisplayVar<Gigantes>("Unsheathe", UnsheatheLoc)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, Hovertip("UNSHEATHE")];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        GigantesPower power = await PowerCmd.Apply<GigantesPower>(context, Owner.Creature, DynamicVars["GigantesPower"].BaseValue, Owner.Creature, this);
        power.Base = DynamicVars["DamageBase"].IntValue;
        power.Addition = DynamicVars["DamageAddition"].IntValue;
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}