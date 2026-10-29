using BaseLib.Abstracts;
using BaseLib.Utils;
using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Tokens;

[Pool(typeof(CurseCardPool))]
public class WhisperingShade() : CustomCardModel(1, CardType.Curse, CardRarity.Curse, TargetType.Self) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(6M)];
    
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext context) {
        await Cmd.Wait(0.25f);
        await GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.IntValue, this, null);
    }
}