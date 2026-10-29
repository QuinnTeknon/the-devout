using Devout.DevoutCode.Powers.Buffs.Performances;
using MegaCrit.Sts2.Core.Commands;

namespace Devout.DevoutCode.Powers.Buffs.Dances.Dances;

public abstract class SongPower : RhythmicPower {
    public override void Animation() {
        VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_spooky_scream");
    }
}