using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Stance.Performance;

public class RighteousRevelry() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CliodnaPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2), new("StrengthSelf", 2m), new PowerVar<StrengthPower>(1m)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        PowerCmd.Apply<StrengthPower>(context, Owner.Creature, DynamicVars["StrengthSelf"].IntValue, Owner.Creature, play.Card);
        foreach (Creature enemy in CombatState.HittableEnemies) {
            PowerCmd.Apply<StrengthPower>(context, enemy, DynamicVars["StrengthPower"].IntValue, Owner.Creature, play.Card);
        }
        await StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CLIODNA, this);
        PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }

    protected override void OnUpgrade() {
        DynamicVars["StrengthSelf"].UpgradeValueBy(1);
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}