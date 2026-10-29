using BaseLib.Utils;
using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Deprecated;

[Pool(typeof(TokenCardPool))]
public class ColdHeart() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await StanceCmd.Switch(context, Owner.Creature, StanceCmd.Stance.NEUTRAL, this);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(4);
}