using System.Drawing;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
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
using Color = Godot.Color;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class SoulflameMaiden() : DevoutCard(1, CardType.Power, CardRarity.Rare, TargetType.AnyEnemy, (0, 4)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(5),
        new("Numerator", 1),
        new("Denominator", 3)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<PlatingPower>(), ..HoverTipFactory.FromEnchantment<Soulflame>()];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        
    }

    protected override void OnUpgrade() {
        piety.UpgradeCostBy(-1);
        DynamicVars["PlatingPower"].UpgradeValueBy(2);
    }
    private static decimal Scaling(CardModel model, Creature? creature) => GeneralCmd.GetEnchantedCards<Soulflame>(creature.Player, PileType.Exhaust).Count * model.DynamicVars["Numerator"].IntValue / model.DynamicVars["Denominator"].IntValue;
}