using Devout.DevoutCode.Patches.Vars;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments;

public class Heavy : DevoutEnchantment {
    public override bool ShowAmount => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HeavyVar(0, false)];
    
    public override bool CanEnchant(CardModel card) => card.Type == CardType.Attack;

    protected override void OnEnchant() {
        DynamicVars["Heavy"].BaseValue = Amount;
    }
}
