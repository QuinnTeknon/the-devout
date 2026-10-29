using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Martyrdom() : DevoutCard(1, CardType.Power, CardRarity.Rare, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<FlagellantPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FlagellantPower>(1)];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await PowerCmd.Apply<TemporaryFlagellantPower>(context, play.Target, DynamicVars["FlagellantPower"].IntValue, Owner.Creature, play.Card);
    }
    
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}