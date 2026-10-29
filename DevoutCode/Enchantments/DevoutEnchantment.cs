using BaseLib.Abstracts;
using BaseLib.Extensions;
using Devout.DevoutCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Localization;

namespace Devout.DevoutCode.Enchantments;

public abstract class DevoutEnchantment(string? heading = null) : CustomEnchantmentModel {
    protected override string? CustomIconPath => $"{(heading != null ? $"{heading}/" : "")}{Id.Entry.RemovePrefix().ToLowerInvariant().TrimSuffix("_enchant")}.png".EnchantmentImagePath();
}