using BaseLib.Utils;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Relics.Ancient.Nonupeipe;

public class SoulboundHeart : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmedPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CharmedPower>()];

    private int ethereal;
    
    public override async Task AfterCardExhausted(PlayerChoiceContext context, CardModel card, bool causedByEthereal) {
        if (card.Owner == Owner) {
            if (card.Enchantment is Soulflame && card.Keywords.Contains(CardKeyword.Ethereal)) {
                if (causedByEthereal) {
                    ethereal++;
                }
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (side == CombatSide.Player && ethereal > 0) {
            Flash();
            await PowerCmd.Apply<CharmedPower>(context, Owner.Creature.CombatState.HittableEnemies, ethereal, Owner.Creature, null);
            ethereal = 0;
        }
    }
}