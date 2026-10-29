using BaseLib.Cards.Variables;
using Devout.DevoutCode.Cards.Angurvadal;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments.Techniques.Templates;

public abstract class TechniqueEnchant(int damage = 0) : DevoutEnchantment("techniques") {
    public decimal damageMult = 1;
    private bool used;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("ExtraDamage", damage), new DisplayVar<TechniqueEnchant>("Angur", CardName)];
    public override bool CanEnchant(CardModel card) => card is Angur;
    public static string CardName(TechniqueEnchant technique) => UnsheatheCard.CardStatic((Angur) (technique.IsMutable ? technique.Card : ModelDb.Card<Angur>())).TitleLocString.GetFormattedText();
    
    public override Decimal EnchantDamageAdditive(Decimal damage, ValueProp props) => (props.IsPoweredAttack() ? Amount : DynamicVars["ExtraDamage"].BaseValue) + ScalingDamage();
    public override Decimal EnchantDamageMultiplicative(Decimal damage, ValueProp props) => props.IsPoweredAttack() ? damageMult : 1m;
    
    public virtual Decimal ScalingDamage() => 0;
    public virtual bool Condition() => false;
    
    protected virtual Task Play(PlayerChoiceContext context, CardPlay play) => Task.CompletedTask;

    public override async Task OnPlay(PlayerChoiceContext context, CardPlay? play) {
        Angur angur = (Angur) Card;
        if (!used) {
            used = true;
            ArgumentNullException.ThrowIfNull(play.Target);
            await Task.Run(() => MainFile.Logger.Info($"Playing technique enchant '{Title.LocEntryKey.Split(".")[0]}'..."));
            TalkCmd.Play(new LocString(locTable, $"{Title.LocEntryKey.Split(".")[0]}.dialogue"), Card.Owner.Creature, VfxColor.Gold);
            await Play(context, play);
        }
        angur.vigor = 0;
        await CardPileCmd.Add(Card, Card.EnergyCost.GetResolved() == 0 ? PileType.Exhaust : PileType.Hand);
        if (play.IsLastInSeries)
            await angur.Disenchant();
    }
}