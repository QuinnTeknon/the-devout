using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Performance;

public class TheWeeping() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>(), HoverTipFactory.FromPower<WeakPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(3), new("WeakSelf", 2m), new PowerVar<WeakPower>(2m)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        PowerCmd.Apply<WeakPower>(context, Owner.Creature, DynamicVars["WeakSelf"].IntValue, Owner.Creature, play.Card);
        foreach (Creature enemy in CombatState.HittableEnemies) {
            PowerCmd.Apply<WeakPower>(context, enemy, DynamicVars["WeakPower"].IntValue, Owner.Creature, play.Card);
        }
        await StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CAOINEAG, this);
        GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, this, play);
    }

    protected override void OnUpgrade() {
        DynamicVars["WeakSelf"].UpgradeValueBy(-1);
        DynamicVars["WeakPower"].UpgradeValueBy(+1);
    }
}