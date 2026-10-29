using BaseLib.Abstracts;
using BaseLib.Extensions;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Powers;

/// <summary>
/// This is the base class for your mod's powers, which is set up to load the power's images from your mod's resources.
/// When creating a power, right click the Powers folder and create a new file with the Custom Power template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
/// </summary>
public abstract class DevoutPower : CustomPowerModel
{
    //Loads from Devout/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();

    /// <summary>
    /// Whether this power is a buff or debuff.
    /// </summary>
    public abstract override PowerType Type { get; }

    /// <summary>
    /// How this power stacks if reapplied. Counter is the most common type, where applying the power again just
    /// adds to the amount. Single means the power does not stack, like Barricade. None functions identically to
    /// Single, but you're suggested to use Single as it is more explicit about how it will work.
    /// </summary>
    public abstract override PowerStackType StackType { get; }
    
    public virtual Task AfterResourceModified(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceSpent(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) => Task.CompletedTask;
    public virtual Task AfterCardEnchanted(PlayerChoiceContext context, CardModel card) => Task.CompletedTask;
    public virtual Task AfterBraidboundStun(BraidboundPower power) => Task.CompletedTask;
    public virtual Task AfterCliodnaEntered(CliodnaPower cliodna) => Task.CompletedTask;
    public virtual Task AfterCaoineagEntered(CaoineagPower caoineag) => Task.CompletedTask;
    public virtual Task AfterAngurPlayed(PlayerChoiceContext context, CardPlay play, Angur angur) => Task.CompletedTask;
    public virtual Task AfterAngurUnsheathed(PlayerChoiceContext context, Angur angur) => Task.CompletedTask;
    
    protected bool CheckResource(string name, SpireResource res, Player chara) => res.getName() == name && chara == Owner.Player;
}