using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Angur.Templates;

public abstract class TechniqueAbstract(int cost, CardRarity rarity, TechniqueEnchant enchant, TechniqueAbstract.Article indefinite = TechniqueAbstract.Article.NONE, bool newline = false, (int, int) flagellant = default, CardType type = CardType.Attack) : UnsheatheCard(cost, type, rarity, TargetType.Self, flagellant) {
    protected override HashSet<CardTag> CanonicalTags => [];
    public override List<(string, string)> Localization => LocManager.Instance.Language switch { _ => new CardLoc(enchant.Title.GetFormattedText(), "{Unsheathe}") };
    
    protected TechniqueEnchant enchant = enchant;
    protected Article indefinite = indefinite;
    protected enum Article { VOWEL, CONSONANT, DEFINITE, NONE }
    protected bool newline = newline;

    protected override bool ShouldGlowGoldInternal => enchant.Condition() && base.ShouldGlowGoldInternal;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DisplayVar<TechniqueAbstract>("Unsheathe", UnsheatheLoc), new DamageVar(0m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, ..enchant.HoverTips];
    public static string UnsheatheLoc(TechniqueAbstract model) =>
        $"{UnsheatheCard.UnsheatheLoc(model)}\nto [gold]Perform[/gold] {model.indefinite switch {Article.VOWEL => "an ", Article.CONSONANT => "a ", Article.DEFINITE => "the ", Article.NONE => "" }}{(model.newline ? "\n" : "")}[purple]{model.enchant.Title.GetFormattedText()}[/purple].";
    
    public override Task Play(PlayerChoiceContext context, CardPlay play) {
        TechniqueAbstract technique = this;
        Creature player = technique.Owner.Creature;
        
        TalkCmd.Play(new LocString(EnchantmentModel.locTable, "FOCUS.dialogue"), Owner.Creature, VfxColor.Gold);
        CreatureCmd.TriggerAnim(player, "Cast", technique.Owner.Character.CastAnimDelay);
        if (Enchantment == null)
            AngurCmd.Unsheathe(Owner, this, upgraded, enchant);
        else
            AngurCmd.Unsheathe(Owner, this, upgraded, enchant,
                damage: new((float) Enchantment.EnchantDamageMultiplicative(1, ValueProp.Move), (float) Enchantment.EnchantDamageAdditive(0, ValueProp.Move)), 
                block: new((float) Enchantment.EnchantBlockMultiplicative(1), (float) Enchantment.EnchantBlockAdditive(0)));
        return GetFlip(out Type? card) != null ? Task.CompletedTask : CardCmd.Exhaust(context, this);
    }
}