using BaseLib.Abstracts;
using BaseLib.Utils;
using Devout.DevoutCode.Character;

namespace Devout.DevoutCode.Potions;

[Pool(typeof(DevoutPotionPool))]
public abstract class DevoutPotion : CustomPotionModel;