using Devout.DevoutCode.Cards.Rewards.Other.Uncommon;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Flagellance;

public class MartyrdomPower : TemporaryFlagellantPower {
    public override AbstractModel OriginModel => ModelDb.Card<Martyrdom>();
}