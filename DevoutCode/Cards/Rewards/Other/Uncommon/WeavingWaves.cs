using BaseLib.Extensions;
using BaseLib.Utils;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

[Pool(typeof(TokenCardPool))]
public class WeavingWaves() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IFlipCard<RagingFlame> {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>(), HoverTipFactory.FromPower<DexterityPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(2m)];
    
    protected override bool ShouldGlowGoldInternal => IsInCorrectStance(StanceCmd.Stance.CAOINEAG);

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        if (ShouldGlowGold)
            await PowerCmd.Apply<DexterityPower>(context, Owner.Creature, DynamicVars["DexterityPower"].IntValue, Owner.Creature, play.Card);
        else
            await StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CAOINEAG, this);
    }

    protected override void OnUpgrade() => DynamicVars["DexterityPower"].UpgradeValueBy(1);
}