using BaseLib.Utils;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Deprecated;

[Pool(typeof(TokenCardPool))]
public class BrokenHeart() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>(), HoverTipFactory.FromPower<ThornsPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ThornsPower>(2m)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CAOINEAG, this, DynamicVars["ThornsPower"].IntValue);
    }

    protected override void OnUpgrade() => DynamicVars["ThornsPower"].UpgradeValueBy(1);
}