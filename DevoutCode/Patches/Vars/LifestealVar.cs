using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Patches.Vars;

public class LifestealVar : DynamicVar {
    public const string Key = "Lifesteal";
    public LifestealVar(int value = 0) : base(Key, value) { }
}