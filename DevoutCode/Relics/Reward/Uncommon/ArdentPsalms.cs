using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Extensions.Resource;
using Devout.DevoutCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Relics.Reward.Uncommon;

public class ArdentPsalms : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override bool ShowCounter => true;

    public override int DisplayAmount {
        get {
            if (!IsActivating)
                return PietyGained % DynamicVars["Piety"].IntValue;
            return DynamicVars["Piety"].IntValue;
        }
    }
    
    private bool IsActivating {
        get => _isActivating;
        set {
            AssertMutable();
            _isActivating = value;
            UpdateDisplay();
        }
    }
    private bool _isActivating;

    [SavedProperty]
    public int PietyGained {
        get => _pietyGained;
        set {
            AssertMutable();
            _pietyGained = value;
            UpdateDisplay();
        }
    }
    private int _pietyGained;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Piety", 12)];
    
    private void UpdateDisplay() {
        if (IsActivating)
            Status = RelicStatus.Normal;
        else
            Status = PietyGained == DynamicVars["Piety"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
        InvokeDisplayAmountChanged();
    }

    public override async Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) {
        if (CheckResource("piety", res, gainer)) {
            PietyGained += amount;
            if (PietyGained >= DynamicVars["Piety"].IntValue) {
                await TaskHelper.RunSafely(DoActivateVisuals());
                CreatureCmd.Heal(gainer.Creature, (int) (info ?? 1));
                PietyGained %= DynamicVars["Piety"].IntValue;
            }
            InvokeDisplayAmountChanged();
        }
    }
    
    private async Task DoActivateVisuals() {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }
}