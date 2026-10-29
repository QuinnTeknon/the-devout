using BaseLib.Extensions;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Extensions;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.angur.powers;

public abstract class SongAbstract<T>(int basic, int upgrade, TargetType target, CardRarity rarity = CardRarity.Uncommon, bool entry = false) : DevoutCard(0, CardType.Skill, rarity, target) where T : PowerModel {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Power", basic)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CaoineagPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [KeywordExtensions.Rhythmic];

    protected override bool ShouldGlowRedInternal => !(entry || IsInCorrectStance(StanceCmd.Stance.CAOINEAG)) || base.ShouldGlowRedInternal;
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        CombatSide side = target switch {
            TargetType.AllAllies => CombatSide.Player,
            TargetType.AllEnemies => CombatSide.Enemy,
            _ => CombatSide.None
        };
        if (entry)
            StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.CAOINEAG, this);
        if (side == CombatSide.None)
            await PowerCmd.Apply<T>(context, play.Target ?? Owner.Creature, DynamicVars["Power"].IntValue, Owner.Creature, play.Card);
        else
            await PowerCmd.Apply<T>(context, CombatState.GetCreaturesOnSide(side), DynamicVars["Power"].IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() {
        DynamicVars["Power"].UpgradeValueBy(upgrade);
    }
}