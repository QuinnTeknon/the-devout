using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Devout.DevoutCode.Cards.Starters;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Relics.Starter.Blood;
using Devout.DevoutCode.Relics.Starter.Eyes;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.Character;

public class Devout : PlaceholderCharacterModel {
    public const string CharacterId = "Devout";

    public static readonly Color Color = new("EEEE00");
    public override Color MapDrawingColor => Color;
    public override Color DialogueColor => Color;
    public override VfxColor SpeechBubbleColor => VfxColor.Gold;

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 90;

    public override Color RemoteTargetingLineColor => new("EFE634");
    public override Color RemoteTargetingLineOutline => new("BFB839");

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeDevout>(),
        ModelDb.Card<StrikeDevout>(),
        ModelDb.Card<StrikeDevout>(),
        ModelDb.Card<StrikeDevout>(),
        ModelDb.Card<PrayDevout>(),
        ModelDb.Card<PrayDevout>(),
        ModelDb.Card<PrayDevout>(),
        ModelDb.Card<PrayDevout>(),
        ModelDb.Card<AngelBlade>(),
        ModelDb.Card<DemonFangs>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics => [
        ModelDb.Relic<BloodOfTheGods>(),
        ModelDb.Relic<EyeOfFire>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<DevoutCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<DevoutRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<DevoutPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon {
        get {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override Color EnergyLabelOutlineColor => Colors.Gold.Blend(new Color("00000088"));
    public override string CustomEnergyCounterPath => "res://Devout/scenes/ui/combat/energy_counter.tscn";

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}