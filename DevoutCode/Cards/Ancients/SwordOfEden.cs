using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.Cards.Ancients;

public class SwordOfEden() : DevoutCard(0, CardType.Power, CardRarity.Ancient, TargetType.Self, (3, 0)) {
    protected override HashSet<CardTag> CanonicalTags => [];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => IsUpgraded ? [HoverTipFactory.FromCard<Angurvadal.Angurvadal>()] : [HoverTipFactory.FromCard<Angurvadal.Angurvadal>(), HoverTipFactory.FromCard<Burn>()];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        SwordOfEden swordOfEden = this;
        Creature player = swordOfEden.Owner.Creature;
        
        NFireBurningVfx fire = NFireBurningVfx.Create(player, 1f, goingRight: false);
        fire.Modulate = Colors.Yellow.Blend(Colors.DarkGoldenrod).Blend(Colors.DarkKhaki);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(fire);
        
        await CreatureCmd.TriggerAnim(player, "Cast", swordOfEden.Owner.Character.CastAnimDelay); 
        await PowerCmd.Apply<SwordOfEdenPower>(context, swordOfEden.Owner.Creature, swordOfEden.DynamicVars.Cards.IntValue, swordOfEden.Owner.Creature, null);
        await AngurCmd.Unsheathe(Owner, this, IsUpgraded);
    }

    protected override void OnUpgrade() {
        DynamicVars.Cards.UpgradeValueBy(-1);
        AddKeyword(CardKeyword.Innate);
        AddKeyword(CardKeyword.Retain);
        
    }
}