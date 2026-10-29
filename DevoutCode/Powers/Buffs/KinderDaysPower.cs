using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs;

public class KinderDaysPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player) {
        foreach (CardModel card in Owner.Player.PlayerCombatState.ExhaustPile.Cards.Where(c => c.Enchantment is Soulflame && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().TakeRandom(Amount, Owner.Player.RunState.Rng.CombatCardGeneration)) {
            await CardCmd.AutoPlay(context, card, null);
            await Task.Run(() => CardCmd.ClearEnchantment(card));
            await GeneralCmd.Enchant(ModelDb.Enchantment<Snuffed>(), card);
        }
    }
}