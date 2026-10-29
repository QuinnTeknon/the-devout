using BaseLib.Abstracts;
using Devout.DevoutCode.Extensions;
using Godot;

namespace Devout.DevoutCode.Character;

public class DevoutRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Devout.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    
    
}