using BaseLib.Abstracts;
using Devout.DevoutCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers.Buffs.Stances;

public abstract class StanceAncilliaryPower<T> : CustomPowerModel where T : PowerModel {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public string title = LocString.GetIfExists("powers", $"{typeof(T).Name.ToSnakeCase().ToUpper()}.title").GetFormattedText();
    public string description = LocString.GetIfExists("powers", $"{typeof(T).Name.ToSnakeCase().ToUpper()}.description").GetFormattedText().TrimPrefix(typeof(T).ToString().Split(".").Last().TrimSuffix("Power") + " ");
    
    public override string CustomPackedIconPath => $"{GetType().ToString().Split(".").Last().ToSnakeCase()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{GetType().ToString().Split(".").Last().ToSnakeCase()}.png".BigPowerImagePath();

    public override List<(string, string)> Localization => LocManager.Instance.Language switch { _ => new CardLoc(
        $"{GetType().ToString().Split(".").Last().ToSnakeCase().Split("_").First().Capitalize()}'s {title}",
        $"While in [gold]{GetType().ToString().Split(".").Last().ToSnakeCase().Split("_").First().Capitalize()}[/gold], {Clause(description, suffix: ".")}"
    )};

    public static string Clause(string power, string? prefix = null, string? suffix = null) {
        power = power.TrimPrefix(prefix ?? "").TrimSuffix(suffix ?? "");
        return $"{power[0].ToString().ToLower()}{power.Substring(1)}";
    }
}