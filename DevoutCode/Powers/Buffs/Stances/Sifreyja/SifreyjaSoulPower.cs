using BaseLib.Abstracts;
using Devout.DevoutCode.Powers.Debuffs;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs.Stances.Sifreyja;

public class SifreyjaSoulPower : StanceAncilliaryPower<DexterityPower> {
    public override async Task BeforeDamageReceived(PlayerChoiceContext context, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source) {
        if (target == Owner && dealer != null && (props.IsPoweredAttack() || source is Omnislice)) {
            Flash();
            await CreatureCmd.Damage(context, dealer, Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, source, null);
        }
    }
    
    public string strength = LocString.GetIfExists("powers", $"{typeof(StrengthPower).Name.ToSnakeCase().ToUpper()}.description").GetFormattedText().TrimPrefix(typeof(StrengthPower).ToString().Split(".").Last().TrimSuffix("Power") + " ");
    public string thorns = LocString.GetIfExists("powers", $"{typeof(ThornsPower).Name.ToSnakeCase().ToUpper()}.description").GetFormattedText().TrimPrefix(typeof(ThornsPower).ToString().Split(".").Last().TrimSuffix("Power") + " ");
    
    public override async Task BeforeSideTurnStart(PlayerChoiceContext context, CombatSide side, IReadOnlyList<Creature> participants, ICombatState state) {
        Flash();
        await PowerCmd.Apply<CharmedPower>(context, state.HittableEnemies, Amount, Owner, null);
    }
    
    public override List<(string, string)> Localization => LocManager.Instance.Language switch { _ => new CardLoc(
        $"{GetType().ToString().Split(".").Last().ToSnakeCase().Split("_").First().Capitalize()}'s Soul",
        $"While in [gold]{GetType().ToString().Split(".").Last().ToSnakeCase().Split("_").First().Capitalize()}[/gold], {Clause(strength, suffix: ".")}, {Clause(thorns, prefix: "", suffix: ".")}, and inflict [gold]Charm[gold] on ALL enemies."
    )};
}