using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class UpslashEnchant() : TechniqueEnchant(5) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<AirbournePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new PowerVar<AirbournePower>(3m)];

    protected override async Task Play(PlayerChoiceContext context, CardPlay? play) {
        AirbournePower power = await PowerCmd.Apply<AirbournePower>(context, play.Target, 1, Card.Owner.Creature, Card);
        power.Force += DynamicVars["AirbournePower"].IntValue;
        PowerCmd.ModifyAmount(context, power, 1, null, null);
        PowerCmd.ModifyAmount(context, power, -1, null, null);
    }
}