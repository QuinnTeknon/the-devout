using BaseLib.Extensions;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Powers.Buffs.Dances;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Veil;

public abstract class VeilAbstract<T>(int basic, int upgrade) : DevoutCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, (0, 1)) where T : VeilPower {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Effect", basic)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [KeywordExtensions.Temporary];

    protected override bool ShouldGlowRedInternal => !IsInCorrectStance(StanceCmd.Stance.CAOINEAG) || base.ShouldGlowRedInternal;

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<T>(context, Owner.Creature, DynamicVars["Effect"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() {
        DynamicVars["Effect"].UpgradeValueBy(upgrade);
    }
}