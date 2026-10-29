using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Patches.Vars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class EndOfTheDanceEnchant() : TechniqueEnchant(5) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new RhythmVar(5)];
    protected override async Task Play(PlayerChoiceContext context, CardPlay? play) {
        Angur angur = (Angur) Card;
        await DamageCmd.Attack(((Angur) Card).DynamicVars.CalculatedDamage.BaseValue + DynamicVars.Damage.BaseValue + angur.vigor).FromCard(Card, play).TargetingAllOpponents(Card.CombatState).WithHitFx("vfx/vfx_giant_horizontal_slash").Execute(context);
    }
    protected override void OnEnchant() {
        ((Angur) Card).AddKeyword(KeywordExtensions.Rhythmic);
    }
}