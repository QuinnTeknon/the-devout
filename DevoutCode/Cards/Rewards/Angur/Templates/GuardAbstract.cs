using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Guards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Angur.Templates;

public abstract class GuardAbstract<G>(int primary = 0, int secondary = 0, int upgrade = 0) : UnsheatheCard(1, CardType.Skill, CardRarity.Common, TargetType.Self, upgradable: false) where G : GuardPower {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new IntVar("Primary", primary), new IntVar("Secondary", secondary)];
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        G power = await PowerCmd.Apply<G>(context, Owner.Creature, DynamicVars["Primary"].IntValue, Owner.Creature, this);
        power.SecondAmount += DynamicVars["Secondary"].IntValue;
        await AngurCmd.Unsheathe(Owner, this, upgraded);
    }
    
    protected override void OnUpgrade() {
        DynamicVars["Primary"].UpgradeValueBy(upgrade);
    }
}