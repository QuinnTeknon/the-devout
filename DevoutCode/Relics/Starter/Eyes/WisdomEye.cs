using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;

namespace Devout.DevoutCode.Relics.Starter.Eyes;

public class WisdomEye : EyeOfFire {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<DexterityPower>()];
    
    public override async Task AfterRoomEntered(AbstractRoom room) {
        if (room is CombatRoom) {
            base.AfterRoomEntered(room);
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, TimesGazed, Owner.Creature, null);
        }
    }
}