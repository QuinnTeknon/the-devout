using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Angur.Templates;

public abstract class UnsheatheCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    (int, int) flagellant = default,
    bool upgradable = true) :
    DevoutCard(cost, type, rarity, target, flagellant)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard(Card(), upgraded)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DisplayVar<UnsheatheCard>("Angur", CardName), new DisplayVar<UnsheatheCard>("Unsheathe", UnsheatheLoc)];
    public static string UnsheatheLoc(UnsheatheCard model) {
        string unsheathe = "Unsheathe";
        string angur = CardName(model);
        return $"[gold]{unsheathe}[/gold] [gold]{angur}{(model is { upgraded: true } ? "+" : "")}[/gold]";
    }

    protected virtual bool upgraded => IsUpgraded && upgradable;
    private bool upgradable = upgradable;
    
    public Tokens.Angurvadal.Angur Card() {
        Tokens.Angurvadal.Angur angur =  ModelDb.Card<Tokens.Angurvadal.Angur>();
        if (CombatState != null) {
            angur = Owner.HasPower<SwordOfEdenPower>() ? ModelDb.Card<Angurvadal.Angurvadal>() : angur;
        }
        return angur;
    }
    public static Tokens.Angurvadal.Angur CardStatic(DevoutCard model) {
        Tokens.Angurvadal.Angur angur =  ModelDb.Card<Tokens.Angurvadal.Angur>();
        if (model.CombatState != null) {
            angur = model.Owner.HasPower<SwordOfEdenPower>() ? ModelDb.Card<Angurvadal.Angurvadal>() : angur;
        }
        return angur;
    }
    public static string CardName(DevoutCard model) => CardStatic(model).TitleLocString.GetFormattedText();
    protected override HoverTip Hovertip(string name, (string, object)[]? vars = null) {
        vars = vars?.Append(("Angur", CardName(this))).ToArray();
        return base.Hovertip(name, vars);
    }
}