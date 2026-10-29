using BaseLib.Extensions;
using Devout.DevoutCode.Extensions.Resource;
using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions {
    public static string replaceLocVars(this string loc, string var, AbstractModel? model = null) {
        IReadOnlyDictionary<string, object> vars = NResourceCounter.getLocString(var).Item1.Variables;
        string l = loc;
        foreach (var (key, value) in vars) {
            l = l.Replace("{single" + key + "}", value.ToString());
            if (model != null) {
                string k = key.Substring(0, 1).ToLower() + key.Substring(1) + "s()}";
                if (l.Contains(k)) {
                    string name = l.Substring(0, l.Find(k) - 1);
                    name = name.Substring(name.RFind("{") + 1);
                    int val = (int) model.GetDynamicVar(name).BaseValue;
                    l = l.Replace("{" + name + ":" + k, String.Join("", Enumerable.Repeat(value, val)));
                }
            }
        }
        return l;
    }
    
    public static string ImagePath(this string path) {
        return Path.Join(MainFile.ResPath, "images", path);
    }

    public static string CardImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "card_portraits", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find card image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "card_portraits", "card.png");
    }

    public static string BigCardImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "card_portraits", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find big card image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "card_portraits", "big", "card.png");
    }

    public static string PowerImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "powers", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find power image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "powers", "power.png");
    }
    
    public static string EnchantmentImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "enchantments", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find enchantment image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "powers", "power.png");
    }

    public static string BigPowerImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "powers", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find big power image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "powers", "big", "power.png");
    }

    public static string RelicImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "relics", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find relic image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "relics", "relic.png");
    }

    public static string BigRelicImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "relics", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find big relic image path: " + path);
        return Path.Join(MainFile.ResPath, "images", "relics", "big", "relic.png");
    }
    
    public static string? RestOptionImagePath(this string path) {
        path = Path.Join(MainFile.ResPath, "images", "ui", "rest_site", path);
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Info("Could not find rest option image path: " + path);
        return null;
    }

    public static string CharacterUiPath(this string path) {
        return Path.Join(MainFile.ResPath, "images", "charui", path);
    }
}