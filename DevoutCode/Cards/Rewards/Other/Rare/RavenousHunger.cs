using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class RavenousHunger() : DevoutCard(2, CardType.Power, CardRarity.Rare, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3), new LifestealVar(2), new PowerVar<StrengthPower>(5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<RingingPower>(), HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<LifestealPower>()];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        
    }

    protected override void OnUpgrade() {
        EnergyCost.UpgradeBy(-1);
    }
}