using BaseLib.Cards.Variables;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Cards.Rewards.Other.Rare;
using Devout.DevoutCode.Powers.Buffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Dualslash() : UnsheatheCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, upgradable: false) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new RepeatVar(1),
        new DisplayVar<Dualslash>("Unsheathe", UnsheatheLoc)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, Hovertip("UNSHEATHE")];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        DualSlashPower power = await PowerCmd.Apply<DualSlashPower>(context, Owner.Creature, DynamicVars["DualSlashPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}