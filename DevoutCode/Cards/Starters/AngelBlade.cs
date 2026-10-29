using BaseLib.Abstracts;
using Devout.DevoutCode.Cards.Ancients;
using Devout.DevoutCode.Cards.Angurvadal;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Commands;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.Cards.Starters;

public class AngelBlade() : UnsheatheCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self, (3, 0)), ITranscendenceCard {
    protected override HashSet<CardTag> CanonicalTags => [];
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard<Burn>(), Hovertip("UNSHEATHE"), Hovertip("PERFORMANCE")];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        AngelBlade angelBlade = this;
        Creature player = angelBlade.Owner.Creature;
        
        NFireBurningVfx fire = NFireBurningVfx.Create(player, 1f, goingRight: false);
        fire.Modulate = Colors.Yellow.Blend(Colors.DarkGoldenrod).Blend(Colors.DarkKhaki);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(fire);

        await CreatureCmd.TriggerAnim(player, "Cast", angelBlade.Owner.Character.CastAnimDelay);
        await AngurCmd.Unsheathe(Owner, this, IsUpgraded);
        await CardPileCmd.AddToCombatAndPreview<Burn>(player,  PileType.Hand, angelBlade.DynamicVars.Cards.IntValue, angelBlade.Owner);
    }
    
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<SwordOfEden>();
}