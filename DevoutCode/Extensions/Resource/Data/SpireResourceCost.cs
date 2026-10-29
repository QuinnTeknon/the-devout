using BaseLib.Abstracts;
using Devout.DevoutCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Extensions.Resource;

public class SpireResourceCost(CardModel card, int value, string name, bool X = false) : CustomResourceCost<SpireResource>(card, value, value == DevoutCard.X || X) {
    public CardModel model = card;
    public string name = name;
    public int Get() {
        //MainFile.Logger.Info($"{card.GetType().ToString().Split(".").Last()}: Canonical({card.IsCanonical}) CostsX({CostsX})");
        return CostsX ? DevoutCard.X : GetWithModifiers(CostModifiers.Local);
    }
}