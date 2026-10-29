using BaseLib.Utils;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Devout.DevoutCode.Cards.Deprecated;

[Pool(typeof(TokenCardPool))]
public class Confession() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Flagellance", 1)];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await PowerCmd.Apply<FlagellantPower>(context, play.Target, 0, Owner.Creature, play.Card);
        FlagellantPower flagellant = play.Target.GetPower<FlagellantPower>();
        flagellant.piety.Gain(DynamicVars["Flagellance"].IntValue);
    }

    protected override void OnUpgrade() {
        DynamicVars["Flagellance"].UpgradeValueBy(1);
    }
}