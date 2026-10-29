using BaseLib.Utils;
using Devout.DevoutCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Relics.Ancient.Tanx;

public class EndlessAppetite : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1m)];

    private int ethereal;
    
    public override async Task AfterCardExhausted(PlayerChoiceContext context, CardModel card, bool causedByEthereal) {
        if (card.Owner == Owner) {
            if (card.Keywords.Contains(CardKeyword.Ethereal) || card.Keywords.Contains(KeywordExtensions.Unsheathed)) {
                if (causedByEthereal) {
                    ethereal++;
                } else {
                    await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, null);
                }
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == CombatSide.Player) {
            await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, DynamicVars["StrengthPower"].BaseValue * ethereal, Owner.Creature, null);
            ethereal = 0;
        }
    }
}