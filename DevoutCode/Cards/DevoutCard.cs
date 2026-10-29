using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Character;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Powers.Debuffs;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.Cards;

[Pool(typeof(DevoutCardPool))]
public abstract class DevoutCard : CustomCardModel {
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    private Random random = new Random();
    public const int X = -2;
    private int options = 3;
    public SpireResourceCost piety;
    public SpireResourceCost penance;
    public int cost;

    public DevoutCard(
        int cost,
        CardType type,
        CardRarity rarity,
        TargetType target,
        (int, int) flagellant = default) :
        base(cost, type, rarity, target)
    {
        piety = new SpireResourceCost(this, flagellant.Item1, "piety");
        penance = new SpireResourceCost(this, flagellant.Item2, "penance");
        this.cost = cost;
    }
    
    protected LocString SelectionScreenPrompt {
        get {
            LocString locString = new LocString("cards", Id.Entry + ".selectionScreenPrompt");
            if (!locString.Exists()) {
                throw new InvalidOperationException($"No selection screen prompt for {Id}.");
            }
            DynamicVars.AddTo(locString);
            return locString;
        }
    }

    public override List<(string, string)>? Localization => new CardLoc(Title, Description.GetFormattedText().replaceLocVars("piety", this).replaceLocVars("penance", this));

    public Type? GetFlip(out Type? card) {
        Type? flip = GetType().GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IFlipCard<>));
        card = flip?.GenericTypeArguments[0] ?? null;
        return flip;
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => GetFlip(out Type? card) != null ? [HoverTipFactory.FromCard(GeneralCmd.CardFromType(card, out CardModel model), IsUpgraded), Hovertip("FLIP", [new("Card", model.TitleLocString.GetFormattedText())])] : [];

    public enum UnplayableReason { NONE, PIETY, PENANCE, FLAGELLANT, CLIODNA, CAOINEAG }
    public UnplayableReason unplayable;

    protected override bool IsPlayable => true;//CanPlay();
    protected override bool ShouldGlowRedInternal => !CanAffordFlagellance();

    protected override bool HasEnergyCostX => cost == X;

    public bool CanAffordFlagellance() {
        unplayable =  UnplayableReason.NONE;
        if (piety.Get() == 0 && penance.Get() == 0)
            return true;
        Creature player = Owner.Creature;
        if (player.HasPower<FlagellantPower>()) {
            FlagellantPower flagellant = player.GetPower<FlagellantPower>();
            if (piety.Get() > flagellant.piety.Amount) {
                unplayable = UnplayableReason.PIETY;
                return false;
            }
            if (penance.Get() > flagellant.penance.Amount) {
                unplayable = UnplayableReason.PENANCE;
                return false;
            }
        } else {
            unplayable = UnplayableReason.FLAGELLANT;
            return false;
        }
        return true;
    }
    
    public bool IsInCorrectStance(StanceCmd.Stance stance) {
        if (stance != StanceCmd.Stance.SIFREYJA) {
            if (stance == StanceCmd.Stance.CLIODNA && !Owner.HasPower<CliodnaPower>()) {
                unplayable = UnplayableReason.CLIODNA;
                return false;
            }
            if (stance == StanceCmd.Stance.CAOINEAG && !Owner.HasPower<CaoineagPower>()) {
                unplayable = UnplayableReason.CAOINEAG;
                return false;
            }
        }
        return true;
    }

    public async Task SpendFlagellance() {
        Creature player = Owner.Creature;
        FlagellantPower flagellant = player.GetPower<FlagellantPower>();
        if (flagellant != null) {
            MainFile.Logger.Info($"SPEND_FLAGELLANCE[{this}] piety: {piety.Get()} ({piety.CostsX}) | penance: {penance.Get()} ({penance.CostsX})");
            if (piety.Get() != 0) {
                if (piety.CostsX)
                    piety.CapturedXValue = flagellant.piety.Amount;
                flagellant.piety.Lose(piety.CostsX ? piety.CapturedXValue : piety.Get());
            }
            if (penance.Get() != 0) {
                if (penance.CostsX)
                    penance.CapturedXValue = flagellant.penance.Amount;
                flagellant.penance.Lose(penance.CostsX ? penance.CapturedXValue : penance.Get());
            }
        }
    }

    public virtual Task Play(PlayerChoiceContext context, CardPlay play) => Task.CompletedTask;

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        if (TargetType.In(TargetType.AnyEnemy, TargetType.AnyAlly, TargetType.Osty))
            ArgumentNullException.ThrowIfNull(play.Target);
        if (play is { IsAutoPlay: false, IsFirstInSeries: true })
            await SpendFlagellance();
        await Task.Run(async() => {
            MainFile.Logger.Info($"TRYING TO PLAY '{GetType().ToString().Split(".").Last()}' FROM '{Pile}'...");
            await Play(context, play);
            if (Enchantment is TechniqueEnchant)
                await Enchantment.OnPlay(context, play);
        });
        if (play is { IsAutoPlay: false, IsFirstInSeries: true })
            GetFlip(out Type? card)?.GetMethod("Flip").Invoke(this, [this]);
        base.OnPlay(context, play);
    }

    public UnplayableReason DisplayInsufficiencyReason() {
        if (unplayable != UnplayableReason.NONE)
            Owner.Creature.GetVfxContainer()?.AddChildSafely(NThoughtBubbleVfx.Create($"{new LocString("unplayability", $"{unplayable.ToString()}.{random.Next(0, options + 1)}").GetFormattedText()}", Owner.Creature, 1.0));
        return unplayable;
    }

    public bool CanPlayInternal() {
        return !ShouldGlowRedInternal;
    }

    public bool CanPlay() {
        bool play = CanPlayInternal();
        if (!play)
            DisplayInsufficiencyReason();
        return play;
    }

    protected override void DeepCloneFields() {
        base.DeepCloneFields();
        piety = new SpireResourceCost(this, piety.Canonical, piety.name, X: piety.CostsX);
        penance = new SpireResourceCost(this, penance.Canonical, penance.name, X: penance.CostsX);
    }
    
    public override bool ShouldPlay(CardModel card, AutoPlayType play) {
        if (card.Keywords.Contains(KeywordExtensions.Enthralling))
            return true;
        return base.ShouldPlay(card, play);
    }
    
    protected virtual HoverTip Hovertip(string name, (string, object)[]? vars = null) {
        LocString title = new LocString("static_hover_tips", $"CAESHA-{name}.title");
        LocString description = new LocString("static_hover_tips", $"CAESHA-{name}.description");
        description.Add("singleEnergyIcon", $"[img]{"text_energy.png".CharacterUiPath()}[/img]");
        if (vars != null) {
            foreach ((var key, var value) in vars) {
                description.Add(key, value.ToString() ?? "");
            }
        }
        return new HoverTip(title, description);
    }
    
    public virtual Task AfterResourceModified(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceSpent(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) => Task.CompletedTask;
    public virtual Task AfterCardEnchanted(PlayerChoiceContext context, CardModel card) => Task.CompletedTask;
    public virtual Task AfterBraidboundStun(BraidboundPower power) => Task.CompletedTask;
    public virtual Task AfterCliodnaEntered(CliodnaPower cliodna) => Task.CompletedTask;
    public virtual Task AfterCaoineagEntered(CaoineagPower caoineag) => Task.CompletedTask;
    public virtual Task AfterAngurPlayed(PlayerChoiceContext context, CardPlay play, Angur angur) => Task.CompletedTask;
    public virtual Task AfterAngurUnsheathed(PlayerChoiceContext context, Angur angur) => Task.CompletedTask;
    
    protected bool CheckResource(string name, SpireResource res, Player chara) => res.getName() == name && chara == Owner;
}