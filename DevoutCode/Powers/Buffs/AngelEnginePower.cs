using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Devout.DevoutCode.Powers.Buffs;

public class AngelEnginePower() : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterEnergyReset(Player player) {
        if (player == Owner.Player) {
            FlagellantPower? flagellant = Owner.GetPower<FlagellantPower>();
            if (Owner.HasPower<FlagellantPower>()) {
                if ((flagellant?.piety.Amount ?? 0) >= Amount) {
                    Flash();
                    PlayerCmd.GainEnergy(Amount, Owner.Player);
                    flagellant.piety.Lose(Amount);
                }
            }
        }
    }
}