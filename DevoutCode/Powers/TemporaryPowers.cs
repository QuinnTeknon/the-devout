using BaseLib.Abstracts;
using Devout.DevoutCode.Cards.Rewards.Bite;
using Devout.DevoutCode.Cards.Rewards.Other.Common;
using Devout.DevoutCode.Enchantments.Techniques;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Guards;
using Devout.DevoutCode.Powers.Buffs.Performances.Dances;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Powers;

public class BoarStrengthPower : CustomTemporaryPowerModelWrapper<BoarGuardPower, StrengthPower>;

public class ShiftStrengthPower : CustomTemporaryPowerModelWrapper<ShiftWeight, StrengthPower>;
public class ShiftDexterityPower : CustomTemporaryPowerModelWrapper<ShiftWeight, DexterityPower>;

public class DanceStrengthPower : CustomTemporaryPowerModelWrapper<DanceOfDeathPower, StrengthPower>;
public class DanceDexterityPower : CustomTemporaryPowerModelWrapper<DanceOfDeathPower, DexterityPower>;

public class FlexStrengthPower : CustomTemporaryPowerModelWrapper<Flex, StrengthPower>;
public class LiltDexterityPower : CustomTemporaryPowerModelWrapper<Lilt, DexterityPower>;

public class HearthStrengthPower : CustomTemporaryPowerModelWrapper<HearthlightPower, StrengthPower>;

public class ParryingThornsPower : CustomTemporaryPowerModelWrapper<ParryingStrikeEnchant, ThornsPower>;