using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class Lilt() : DevoutCard(1, CardType.Skill, CardRarity.Common, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>(), HoverTipFactory.FromPower<DexterityPower>(), HoverTipFactory.FromPower<WeakPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(4m), new PowerVar<WeakPower>(2m)];
    
    protected override bool ShouldGlowGoldInternal => IsInCorrectStance(StanceCmd.Stance.CAOINEAG);

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        if (ShouldGlowGold) {
            await PowerCmd.Apply<LiltDexterityPower>(context, Owner.Creature, DynamicVars["DexterityPower"].IntValue, Owner.Creature, play.Card);
            await PowerCmd.Apply<WeakPower>(context, Owner.Creature, DynamicVars["WeakPower"].IntValue, Owner.Creature, play.Card);
        }
    }

    protected override void OnUpgrade() => DynamicVars["DexterityPower"].UpgradeValueBy(2);
}