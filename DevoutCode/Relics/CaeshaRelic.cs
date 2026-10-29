using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Character;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Relics;

/// <summary>
/// This is the base class for your mod's relics, which is set up to load the relic's images from your mod's resources.
/// When creating a relic, right click the Relics folder and create a new file with the Custom Relic template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
///
/// The [Pool] annotation marks this relic as being tied to your specific character. Inheriting from this class means
/// that your relics don't need to individually say which pool they should be in.
/// </summary>
[Pool(typeof(DevoutRelicPool))]
public abstract class DevoutRelic : CustomRelicModel {
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
    
    public override List<(string, string)>? Localization => new CardLoc(Title.GetFormattedText(), DynamicDescription.GetFormattedText().replaceLocVars("piety", this).replaceLocVars("penance", this));
    
    protected virtual HoverTip Hovertip(string name, (string, object)[]? vars = null) {
        LocString title = new LocString("static_hover_tips", $"DEVOUT-{name}.title");
        LocString description = new LocString("static_hover_tips", $"DEVOUT-{name}.description");
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