using Devout.DevoutCode.Powers.Buffs.Dances;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace Devout.DevoutCode.Powers.Buffs.Performances.Dances;

public abstract class DancePower : RhythmicPower {
    public override void Animation() {
        Color color = new Color("FFFFFF80");
        double num = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(color, 0.8 + 4 * num));
    }
}