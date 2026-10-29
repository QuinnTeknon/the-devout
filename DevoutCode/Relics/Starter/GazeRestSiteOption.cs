using Devout.DevoutCode.Relics.Starter.Eyes;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace Devout.DevoutCode.Relics.Starter;

public class GazeRestSiteOption : RestSiteOption {
    public override string OptionId => "GAZE";

    public RelicModel?[] GazeRelics => [Owner.GetRelic<StoneHumidifier>(), Owner.GetRelic<RegalPillow>()];
    
    public override LocString Description {
        get {
            EyeOfFire relic = Owner.GetRelic<EyeOfFire>();
            
            LocString extraText = new LocString("rest_site_ui", $"OPTION_{OptionId}.extraText");
            extraText.Add("Gazed", relic.TimesGazed);
            extraText.Add("WisdomEyes", relic is WisdomEye ? " and [gold]Dexterity[/gold]" : "");
            extraText.Add("Strength", relic.DynamicVars["StrengthPower"].BaseValue);
            
            LocString description = base.Description;
            description.Add("Character", Owner.Character.Id.Entry);
            string text = "\n" + extraText.GetFormattedText();
            ForEachGazeRelic((r, g) => {
                LocString? test = LocString.GetIfExists("relic_replacements", r.Id.Entry + ".additionalRestSiteGazeText");
                if (test != null) {
                    foreach (KeyValuePair<string, object> var in r.DynamicDescription.Variables)
                        test.Add(var.Key, var.Value?.ToString() ?? "");
                    text += $"\n{test.GetFormattedText()}";
                }
                return Task.CompletedTask;
            });
            description.Add("ExtraText", text);
            return description;
        }
    }
    
    public GazeRestSiteOption(Player owner) : base(owner) { }
    
    public override async Task<bool> OnSelect() {
        Owner.GetRelic<EyeOfFire>().TimesGazed++;
        ForEachGazeRelic((r, g) => r.AfterRestSiteHeal(g.Owner, true));
        return await Task.FromResult(result: true);
    }
    
    public override async Task DoLocalPostSelectVfx(CancellationToken ct = default) {
        NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
        NRestSiteRoom.Instance?.AddChildSafely(NRestSmokeVfx.Create());
        NRestSiteRoom.Instance?.AddChildSafely(NDesaturateTransitionVfx.Create());
        await Cmd.CustomScaledWait(1.5f, 2.5f, ignoreCombatEnd: false, ct);
    }
    
    public override Task DoRemotePostSelectVfx() {
        NRestSiteCharacter nRestSiteCharacter = NRestSiteRoom.Instance?.Characters.First(c => c.Player == Owner);
        nRestSiteCharacter?.Shake();
        NRelicFlashVfx nRelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<EyeOfFire>());
        if (nRelicFlashVfx == null)
            return Task.CompletedTask;
        nRestSiteCharacter?.AddChildSafely(nRelicFlashVfx);
        nRelicFlashVfx.Position = Vector2.Zero;
        return Task.CompletedTask;
    }

    public void ForEachGazeRelic(Func<RelicModel, GazeRestSiteOption, Task> action) {
        foreach (RelicModel? relic in GazeRelics) {
            if (relic is not null)
                action(relic, this);
        }
    }
}