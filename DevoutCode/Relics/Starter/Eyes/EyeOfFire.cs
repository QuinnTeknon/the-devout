using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace Devout.DevoutCode.Relics.Starter.Eyes;

public class EyeOfFire : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<StrengthPower>()];

    private int _timesGazed;
    public override bool ShowCounter => true;
    public override int DisplayAmount => TimesGazed;

    [SavedProperty]
    public int TimesGazed {
        get => _timesGazed;
        set {
            AssertMutable();
            _timesGazed = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room) {
        if (room is CombatRoom) {
            Flash();
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, TimesGazed, Owner.Creature, null);
        }
    }

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options) {
        if (player != Owner)
            return false;
        var list = options.ToList();
        options.Clear();
        options.Add(new GazeRestSiteOption(player));
        foreach (RestSiteOption option in list) {
            if (option is not HealRestSiteOption) {
                options.Add(option);
            }
        }
        return true;
    }
    
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<WisdomEye>();
}