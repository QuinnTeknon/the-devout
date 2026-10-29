using BaseLib.Abstracts;
using BaseLib.Utils;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace Devout.DevoutCode.Relics.Test;

[Pool(typeof(SharedRelicPool))]
public class WeightedBelt : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Shop;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new HeavyVar(2m)];

    public override async Task AfterObtained() {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue) {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        Heavy enchantment = ModelDb.Enchantment<Heavy>();
        foreach (CardModel item in await CardSelectCmd.FromDeckForEnchantment(Owner, enchantment, DynamicVars["Heavy"].IntValue, prefs)) {
            CardCmd.Enchant(enchantment.ToMutable(), item, DynamicVars["Heavy"].IntValue);
            CardCmd.Preview(item);
        }
    }
}