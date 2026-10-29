using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Patches.Vars;

public class HeavyVar : DynamicVar {
    public const string Key = "Heavy";
    public bool keyword;

    public HeavyVar(decimal heavy, bool keyword = true, bool showTooltip = true) : base("Heavy", heavy) {
        this.keyword = keyword;
        if (keyword && showTooltip) this.WithTooltip();
    }
}