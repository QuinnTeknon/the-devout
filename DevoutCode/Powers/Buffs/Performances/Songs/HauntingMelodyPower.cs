using Devout.DevoutCode.Powers.Buffs.Dances;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Performances.Songs;

public class HauntingMelodyPower : RhythmicPower {
    public override async Task Activate(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.GainBlock(Owner, Amount + GetDexterityModifier(Owner), ValueProp.Unpowered, null);
    }
}