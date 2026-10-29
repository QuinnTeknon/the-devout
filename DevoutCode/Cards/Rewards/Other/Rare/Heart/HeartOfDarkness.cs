using BaseLib.Abstracts;
using BaseLib.Utils;
using Devout.DevoutCode.Cards.Rewards.Angur;
using Devout.DevoutCode.Character;
using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Heart;

[Pool(typeof(TokenCardPool))]
public class HeartOfDarkness() : DevoutCard(1, CardType.Curse, CardRarity.Curse, TargetType.Self), IFlipCard<SacredHeart> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(6), new EnergyVar(1)];
    
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext context) {
        await Cmd.Wait(0.25f);
        foreach (Creature c in Owner.Creature.CombatState.Creatures.Where(c=> !c.IsPet)) {
            await GeneralCmd.LoseHP(context, c, DynamicVars.HpLoss.IntValue, this, null);
        }
        await Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade() {
        AddKeyword(CardKeyword.Retain);
    }
}