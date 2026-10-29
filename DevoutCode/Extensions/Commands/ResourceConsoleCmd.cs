using System.Text.RegularExpressions;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Devout.DevoutCode.Extensions.Commands;

public class ResourceConsoleCmd : AbstractConsoleCmd {
    public override string CmdName => "resource";
    public override string Args => "<resource:string> <amount:int>";
    public override string Description => "Adds a particular resource to player";
    public override bool IsNetworked => true;

    public override CmdResult Process(Player? issuingPlayer, string[] args) {
        if (args.Length == 0)
            return new CmdResult(success: false, "The first argument must be a string.");
        if (issuingPlayer == null)
            return new CmdResult(success: false, "This command only works during a run.");
        if (issuingPlayer.PlayerCombatState == null)
            return new CmdResult(success: false, "This command only works in combat.");
        FlagellantPower? flagellant = issuingPlayer.Creature.GetPower<FlagellantPower>();
        if (flagellant == null)
            return new CmdResult(success: false, "The player must be suitable to hold resources.");
        SpireResource? resource = (SpireResource?) AccessTools.Field(typeof(FlagellantPower), args[0])?.GetValue(flagellant);
        if (resource == null)
            return new CmdResult(success: false, $"The resource '{args[0]}' does not exist.");
        if (int.TryParse(args[1], out var result)) {
            if (result < 0)
                return new CmdResult(success: false, "The resource amount cannot be negative.");
            resource.Gain(result);
            return new CmdResult(success: true, $"Added '{result}' {args[0]}.");
        }
        return new CmdResult(success: false, "The second argument must be an int.");
    }
}
