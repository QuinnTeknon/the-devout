using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class Inquisition() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, (2, 0)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(2),
        new PowerVar<StrengthPower>(1),
        new CardsVar(1)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard<Burn>(), HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_gaze");
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_spooky_scream");
        PowerCmd.Apply<StrengthPower>(context, Owner.Creature, DynamicVars.Strength.IntValue, Owner.Creature, play.Card);
        PowerCmd.Apply<VulnerablePower>(context, play.Target, DynamicVars.Vulnerable.IntValue, Owner.Creature, play.Card);
        GeneralCmd.CreateEnchantedCards<Burn>(IsUpgraded ? ModelDb.Enchantment<Soulflame>() : null, PileType.Hand, CardPilePosition.Bottom, DynamicVars.Cards.BaseValue, this);
    }

    protected override void OnUpgrade() {
        foreach (IHoverTip tip in HoverTipFactory.FromEnchantment<Soulflame>()) {
            ExtraHoverTips.AddItem(tip);
        }
    }
}