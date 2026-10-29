using BaseLib.Abstracts;
using Devout.DevoutCode.Cards.Tokens;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Ancients;

public class OriginalSin() : DevoutCard(1, CardType.Power, CardRarity.Ancient, TargetType.Self, (0, 0)), ITomeCard {
    protected override HashSet<CardTag> CanonicalTags => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate, KeywordExtensions.Enthralling, CardKeyword.Eternal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new IntVar("Flagellant", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard<WhisperingShade>(), HoverTipFactory.FromPower<FlagellantPower>()];
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        OriginalSin originalSin = this;
        Creature player = originalSin.Owner.Creature;
        
        await CreatureCmd.TriggerAnim(player, "Cast", originalSin.Owner.Character.CastAnimDelay); 
        await PowerCmd.Apply<FlagellantPower>(context, originalSin.Owner.Creature, originalSin.DynamicVars["Flagellant"].IntValue, originalSin.Owner.Creature, null);
        await PowerCmd.Apply<OriginalSinPower>(context, originalSin.Owner.Creature, originalSin.DynamicVars["Flagellant"].IntValue, originalSin.Owner.Creature, null);
    }
    
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}