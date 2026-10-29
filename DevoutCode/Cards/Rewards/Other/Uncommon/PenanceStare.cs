using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class PenanceStare() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, (1, 0)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(2),
        new DamageVar(12, ValueProp.Move),
        new CardsVar(1)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard<Burn>(), HoverTipFactory.FromPower<VulnerablePower>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        NFireBurningVfx fire = NFireBurningVfx.Create(Owner.Creature, 1f, goingRight: true);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(fire);
        
        DamageCmd.Attack(DynamicVars.Damage.IntValue).FromCard(this, play).Targeting(play.Target).Execute(context);
        await PowerCmd.Apply<VulnerablePower>(context, play.Target, DynamicVars.Vulnerable.IntValue, Owner.Creature, play.Card);
        Soulflame soulflame = IsUpgraded ? ModelDb.Enchantment<Soulflame>() : null;
        GeneralCmd.CreateEnchantedCards<Burn>(soulflame, PileType.Hand, CardPilePosition.Bottom, DynamicVars.Cards.BaseValue, this);
        GeneralCmd.CreateEnchantedCards<Burn>(soulflame, PileType.Draw, CardPilePosition.Bottom, DynamicVars.Cards.BaseValue, this);
        GeneralCmd.CreateEnchantedCards<Burn>(soulflame, PileType.Discard, CardPilePosition.Bottom, DynamicVars.Cards.BaseValue, this);
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1);
        foreach (IHoverTip tip in HoverTipFactory.FromEnchantment<Soulflame>()) {
            ExtraHoverTips.AddItem(tip);
        }
    }
}