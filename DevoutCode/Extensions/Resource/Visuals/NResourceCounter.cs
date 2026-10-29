using BaseLib.Extensions;
using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Patches;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace Devout.DevoutCode.Extensions.Resource;

public partial class NResourceCounter : Control {
	
	private static readonly StringName _v = new StringName("v");
	private static readonly StringName _s = new StringName("s");

	private static readonly string _gainVfxPath = SceneHelper.GetScenePath("vfx/star_gain_vfx");

	public Player? _player;
	public SpireResource? _resource;
	public Vector2 _position;
	public int _rotation;
	public int _zIndex;

	private MarginContainer? _container;
	private RichTextLabel? _label;
	private Control? _rotationLayers;
	private Control? _icon;
	public LocString _locString;
	private HoverTip _hoverTip;
	
	private ShaderMaterial? _hsv;
	private Tween? _hsvTween;
	private Tween? _squeezeTween;

	private int _displayedResourceCount;
	private float _lerpingResourceCount;
	private float _velocity;

	private bool _isListeningToCombatState;
	private bool _themeOverridden;

	public void DefineDescendants() {
		_label = GetNode<RichTextLabel>("Visual/Count/Label");
		_rotationLayers = GetNode<Control>("Visual/Icon/RotationLayers");
		_icon = GetNode<Control>("Visual/Icon");
		_hsv = (ShaderMaterial)_icon.Material;
		Visible = false;
		MainFile.Logger.Info($"Descendants are {_label}, {_rotationLayers}, {_icon}, and {_hsv}.");
	}
	
	public override void _Ready() {
		SetPosition(new Vector2(64, 1145) + _position);
		
		MainFile.Logger.Info($"Name is '{GetName()}' | Parent: {GetParent().GetType().Name} {(((Control) GetParent()).Visible ? "⛊" : "⛉")}: {((Control) GetParent()).GlobalPosition} | Descendants: {FindChildren("*", owned: false).Select(child => $"{child.GetType().Name} {(((Control) child).Visible ? "⛊" : "⛉")}: {((Control) child).GlobalPosition}").ToArray().Join()}");
		DefineDescendants();
		_icon.Rotation = Single.DegreesToRadians(_rotation);
		_icon.ZIndex = _zIndex;
		
		(LocString, LocString) locStrings = getLocString(_resource.getName());
		_locString = locStrings.Item1;
		_hoverTip = new HoverTip(locStrings.Item2, locStrings.Item1);
		Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
		Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
	}

	public override void _Process(double delta) {
		Visible = _player != null;
		if (Visible) {
			if (!_themeOverridden) {
				_label.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, Colors.White);
				_label.AddThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor, _player.Character.EnergyLabelOutlineColor);
				_themeOverridden = true;
			}
			float num = _resource.Amount == 0 ? 5f : 30f;
			for (int i = 0; i < _rotationLayers.GetChildCount(); i++) {
				_rotationLayers.GetChild<Control>(i).RotationDegrees += (float)delta * num * (float)(i + 1);
			}
			_lerpingResourceCount = MathHelper.SmoothDamp(_lerpingResourceCount, _resource.Amount, ref _velocity, 0.1f, (float)delta);
			SetResourceCountText(Mathf.RoundToInt(_lerpingResourceCount));
		}
	}
	
	public void Initialize(Player player) {
		_player = player;
		RefreshVisibility();
	}

	private void OnHovered() {
		NHoverTipSet.CreateAndShow(this, _hoverTip)?.SetGlobalPosition(GlobalPosition + new Vector2(-34f, -300f));
	}

	private void OnUnhovered() {
		NHoverTipSet.Remove(this);
	}

	private void OnResourceChanged(int oldResource, int newResource) {
		UpdateResourceCount(oldResource, newResource);
		RefreshVisibility();
	}

	private void UpdateResourceCount(int oldCount, int newCount) {
		if (Math.Sign(oldCount - newCount) != 0) {
			_hsvTween?.Kill();
			_squeezeTween?.Kill();
			_squeezeTween = CreateTween();
			_squeezeTween.TweenMethod(Callable.From<Vector2>(UpdateSqueeze), new Vector2(1.2f, 0.8f), Vector2.One, 1);
			if (newCount < oldCount) {
				_hsv.SetShaderParameter(_v, 1f);
				_lerpingResourceCount = newCount;
				SetResourceCountText(newCount);
			} else if (newCount > oldCount) {
				_hsvTween = CreateTween();
				_hsvTween.TweenMethod(Callable.From<float>(UpdateShaderV), 2f, 1f, 0.2);
				Node2D node2D = PreloadManager.Cache.GetAsset<PackedScene>(_gainVfxPath)
					.Instantiate<Node2D>();
				this.AddChildSafely(node2D);
				this.MoveChildSafely(node2D, 0);
				node2D.Position = Size / 2f;
			}
		}
	}

	private void SetResourceCountText(int Resource) {
		if (_displayedResourceCount != Resource) {
			_displayedResourceCount = Resource;
			_label.AddThemeColorOverride(ThemeConstants.Label.FontColor, (Resource == 0) ? StsColors.red : StsColors.cream);
			_label.Text = $"[center]{Resource}[/center]";
			if (Resource == 0) {
				_hsv.SetShaderParameter(_s, 0.5f);
				_hsv.SetShaderParameter(_v, 0.85f);
			} else {
				_hsv.SetShaderParameter(_s, 1f);
				_hsv.SetShaderParameter(_v, 1f);
			}
		}
	}

	private void UpdateSqueeze(Vector2 value) {
		_label.Scale = value;
	}
	private void UpdateShaderV(float value) {
		_hsv.SetShaderParameter(_v, value);
	}

	private void RefreshVisibility() {
		if (_player == null) {
			Visible = false;
			return;
		}
		int Resource = _resource.Amount;
		Visible = Visible || _player.HasPower<FlagellantPower>() || Resource > 0;
	}

	public static (LocString, LocString) getLocString(string name) {
		string count = $"{name.ToUpper()}_COUNT";
		LocString locString = new LocString("static_hover_tips", $"{count}.description");
		locString.Add($"{name.Capitalize()}Icon", $"[img]res://Devout/images/packed/sprite_fonts/{name}_icon.png[/img]");
		LocString hovString = new LocString("static_hover_tips", $"{count}.title");
		//MainFile.Logger.Info($"{count}: '{locString.GetFormattedText()}'\n{locString.Variables}");
		return (locString, hovString);
	}
}
