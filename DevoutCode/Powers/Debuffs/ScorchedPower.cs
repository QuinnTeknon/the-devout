using Devout.DevoutCode.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Devout.DevoutCode.Powers.Debuffs;

public class ScorchedPower() : DevoutPower {
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}