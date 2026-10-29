using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Forms;

public class SeraphFormPower : AscendingPower {
    public override Task Crash(PlayerChoiceContext context) => GeneralCmd.EnchantCardsInPile(context, ModelDb.Enchantment<Soulflame>(), PileType.Hand, 1, this, true);

    public override async Task BeforeSideTurnStart(PlayerChoiceContext context, CombatSide side, IReadOnlyList<Creature> participants, ICombatState state) {
        if (IsGrounded) {
            Flash();
            IsGrounded = false;
            await PowerCmd.ModifyAmount(context, this, DynamicVars["Wings"].IntValue - Amount, null, null);
        }
    }
}