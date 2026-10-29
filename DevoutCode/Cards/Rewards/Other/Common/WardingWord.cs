using Devout.DevoutCode.Cards.Rewards.Other.Uncommon;
using Devout.DevoutCode.Powers.Debuffs;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Common;

public class WardingWord() : DevoutCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, (1, 0)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6M, ValueProp.Move), new PowerVar<CharmedPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<CharmedPower>()];
    
    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e=> e.HasPower<CharmedPower>()) ?? false;
    
    public override async Task AfterCardChangedPiles(CardModel card, PileType pile, AbstractModel? clonedBy) {
        if (card is Liturgy && card.ShouldGlowGold)
            if (pile == PileType.Play)
                card.BaseReplayCount -= 1;
            else if (card.Pile.Type == PileType.Play)
                card.BaseReplayCount += 1;
    }
    
    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NScratchVfx.Create(t, true))).Execute(context);
        PowerCmd.Apply<CharmedPower>(context, play.Target, DynamicVars["CharmedPower"].IntValue, Owner.Creature, play.Card);
    }
    
    protected override void OnUpgrade() {
        DynamicVars["CharmedPower"].UpgradeValueBy(1);
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}