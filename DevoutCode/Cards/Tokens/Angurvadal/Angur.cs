using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Devout.DevoutCode.Character;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Tokens.Angurvadal;

[Pool(typeof(TokenCardPool))]
public class Angur() : DevoutCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [KeywordExtensions.Unsheathed];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HeavyVar(2),
        new DamageVar(10, ValueProp.Move),
        new DisplayVar<Angur>("TrueDamage", card=> $"{(card.CombatState != null ? (card.Enchantment != null ? (card.Owner.GetRelic<MysticLighter>() != null ? $"[blue]{DamageLoc(card)}[/blue]" : $"[purple]{DamageLoc(card)}[/purple]") : (DamageLoc(card) < card.DynamicVars.Damage.BaseValue ? $"[red]{DamageLoc(card)}[/red]" : (DamageLoc(card) > card.DynamicVars.Damage.BaseValue ? $"[green]{DamageLoc(card)}[/green]" : DamageLoc(card)))) : card.DynamicVars.Damage.BaseValue)}")
    ];

    public bool unsheathed;
    public int strikes;

    public int vigor;

    public Creature? target;

    protected override bool ShouldGlowGoldInternal => Enchantment is TechniqueEnchant technique && technique.Condition() && base.ShouldGlowGoldInternal;

    public static IEnumerable<Creature> GetRunState(Angur angur) {
        List<Creature> creatures = [angur.Owner.Creature];
        if (angur.target != null) creatures.Append(angur.target);
        return creatures;
    }
    public static int DamageLoc(Angur angur) {
        int damage = (int) angur.BaseDamage();
        IEnumerable<AbstractModel> modifiers = [];
        damage = (int) Hook.ModifyDamage(
                            IRunState.GetFrom(GetRunState(angur)), 
                            angur.CombatState, 
                            angur.target, 
                            angur.Owner.Creature, 
                            damage, ValueProp.Move, 
                            angur, null, 
                            ModifyDamageHookType.All, CardPreviewMode.None, 
                            out modifiers);
        MainFile.Logger.Info($"ANGUR_DAMAGE[Turn {angur.Owner.PlayerCombatState.TurnNumber}]: {damage} | {angur.target}: {String.Join(", ", modifiers)} | {String.Join(", ", new object[] { 
            $"Add: {angur.Enchantment?.EnchantDamageAdditive(0m, ValueProp.Move)}",
            $"Mult: {angur.Enchantment?.EnchantDamageMultiplicative(1m, ValueProp.Move)}",
            $"Strength: {angur.Owner.Creature.GetPowerAmount<StrengthPower>() + angur.Owner.Creature.GetPowerAmount<CliodnaStrengthPower>()}",    
            $"Heavy: {angur.DynamicVars["Heavy"].IntValue}",
            $"Titanicus: {angur.Owner.Creature.GetPowerAmount<TitanicusPower>()}" })
        }");
        return damage;
    }
    public virtual decimal BaseDamage() => DynamicVars.Damage.BaseValue;

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        Angur angur = this;
        vigor = Owner.Creature.GetPowerAmount<VigorPower>();
        AttackCommand command = DamageCmd.Attack(angur.BaseDamage());
        await command.FromCard(angur, play).Targeting(play.Target).WithHitVfxNode(NBigSlashVfx.Create).WithHitVfxNode(NBigSlashImpactVfx.Create).Execute(context);
        AngurCmd.AfterAngurPlayed(context, play, angur);
        strikes += 1;
    }
    
    public override Task BeforeSideTurnEndEarly(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
        if (Pile is not { Type: PileType.Exhaust }) {
            strikes = 0;
            return CardCmd.Exhaust(context, this, true);
        }
        return Task.CompletedTask;
    }
    
    protected override CardLocation GetResultLocationForCardPlay() {
        if (EnergyCost.GetResolved() == 0) {
            strikes = 0;
            return new CardLocation(Owner, PileType.Exhaust, CardPilePosition.Bottom);
        }
        return new CardLocation(Owner, PileType.Hand, CardPilePosition.Top);
    }

    public override async Task AfterCardChangedPilesLate(CardModel card, PileType old, AbstractModel? clonedBy) {
        if ((CombatManager.Instance.History.CardPlaysFinished.LastOrDefault()?.CardPlay.Card ?? null) == this) {
            await Disenchant();
        }
    }

    protected override void OnUpgrade() {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.Damage.UpgradeValueBy(5);
        DynamicVars["Heavy"].UpgradeValueBy(1);
    }

    public async Task Disenchant() {
        CardCmd.ClearEnchantment(this);
        Angur replacement = (Angur) ModelDb.AllCards.First(c => c.GetType() == GetType()).ToMutable();
        //replacement.target = null;
        //target = null;
        await Task.Run(() => CombatState.AddCard(replacement, Owner));
        await CardCmd.Transform(this, replacement, CardPreviewStyle.None);
    }

    public async Task Return() {
        CardLocation pile = GetResultLocationForCardPlay();
        //CardPileCmd.Add(this, GeneralCmd.GetPileFromType(Owner, pile.pileType), pile.position);
    }
}