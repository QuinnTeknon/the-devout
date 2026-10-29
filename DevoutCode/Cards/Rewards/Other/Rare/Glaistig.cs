using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Patches.Vars;
using Devout.DevoutCode.Powers.Buffs.Dances;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare;

public class Glaistig() : DevoutCard(1, CardType.Power, CardRarity.Rare, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GlaistigPower>(1), new PowerVar<DexterityPower>(2), new RhythmVar(5)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        if (IsUpgraded)
            await PowerCmd.Apply<DexterityPower>(context, Owner.Creature, DynamicVars["DexterityPower"].IntValue, Owner.Creature, play.Card);    
        await PowerCmd.Apply<GlaistigPower>(context, Owner.Creature, DynamicVars["GlaistigPower"].IntValue, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade() {
        EnergyCost.UpgradeBy(-1);
    }
}