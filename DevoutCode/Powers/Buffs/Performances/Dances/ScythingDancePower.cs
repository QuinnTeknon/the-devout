using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Performances.Dances;

public class ScythingDancePower : DancePower {
    public override async Task Activate(PlayerChoiceContext context, CardPlay play) {
        foreach (Creature enemy in CombatState.HittableEnemies) {
            await CreatureCmd.Damage(context, enemy, Amount + GetDexterityModifier(Owner), ValueProp.Unpowered | ValueProp.Move, play.Card, play);
        }
    }
}