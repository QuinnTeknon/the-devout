using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon;

public class HymnOfSanctuary() : DevoutCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, (2, 0)) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharmedPower>(3)];
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
        PowerCmd.Apply<CharmedPower>(context, CombatState?.HittableEnemies, DynamicVars["CharmedPower"].IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() => DynamicVars["CharmedPower"].UpgradeValueBy(2M);
}