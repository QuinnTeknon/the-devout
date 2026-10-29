using Devout.DevoutCode.Powers.Buffs.Dances.Dances;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Powers.Buffs.Performances.Songs;

public class DirgingSongPower : SongPower {
    public int SumBy => Amount + GetDexterityModifier(Owner);
    
    public override async Task Activate(PlayerChoiceContext context, CardPlay play) {
        foreach (Creature enemy in CombatState.HittableEnemies) {
            await PowerCmd.Apply<CharmedPower>(context, enemy, SumBy, play.Card.Owner.Creature, play.Card);
        }
    }
}