using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.CoOp;

public class BrilliantSmile() : DevoutCard(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(7), new("Threshold", 5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CliodnaPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override bool ShouldGlowRedInternal => !IsInCorrectStance(StanceCmd.Stance.CLIODNA) || base.ShouldGlowRedInternal;
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        decimal heal = GeneralCmd.GetDualPowerModifier<StrengthPower, CliodnaStrengthPower>(Owner.Creature, DynamicVars["Threshold"].IntValue);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        foreach (Creature c in Owner.Creature.CombatState.Creatures.Where(c=> !c.IsPet)) {
            await CreatureCmd.Heal(c, heal);
        }
    }

    protected override void OnUpgrade() {
        DynamicVars.Heal.UpgradeValueBy(3m);
    }
}