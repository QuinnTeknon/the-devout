using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Flagellance() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, ..HoverTipFactory.FromEnchantment<Soulflame>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(4), new CardsVar(2)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.IntValue, this, play);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
        await GeneralCmd.EnchantCardsInPile(context, ModelDb.Enchantment<Soulflame>(), PileType.Hand, DynamicVars.Cards.BaseValue, this);
    }

    protected override void OnUpgrade() {
        DynamicVars.HpLoss.UpgradeValueBy(-1);
        DynamicVars.Cards.UpgradeValueBy(+1);
    }
}