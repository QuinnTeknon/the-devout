using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Patches.Vars;

public class RhythmVar : DynamicVar {
    public const string Key = "Rhythm";
    public RhythmVar(decimal rhythm) : base(Key, rhythm) { this.WithTooltip(); }
}