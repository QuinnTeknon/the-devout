using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs;

public class BoilingBraidPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardEnchanted(PlayerChoiceContext context, CardModel card) => await Scald(context, card);
    public override async Task AfterCardDrawn(PlayerChoiceContext context, CardModel card, bool fromHandDraw) => await Scald(context, card);
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay play) => await Scald(context, play.Card);

    public async Task Scald(PlayerChoiceContext context, CardModel card) {
        if (card.Enchantment is Soulflame && card.Owner.Creature == Owner) {
            IEnumerable<Creature> hittableEnemies = CombatState.HittableEnemies.Where(c => c.GetPowerAmount<BraidboundPower>() > 0);
            if (hittableEnemies.Count() != 0)
                await CreatureCmd.Damage(context, hittableEnemies, card.Enchantment.Amount * Amount, ValueProp.Unblockable | ValueProp.Unpowered, null);
        }
    }
}