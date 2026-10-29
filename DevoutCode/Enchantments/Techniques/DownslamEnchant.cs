using Devout.DevoutCode.Enchantments.Techniques.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class DownslamEnchant() : TechniqueEnchant(5) {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, StunIntent.GetStaticHoverTip(), HoverTipFactory.FromPower<RingingPower>()];

    public override bool Condition() => Card.CombatState?.HittableEnemies.Any(e=> e.HasPower<AirbournePower>() || e.HasPower<SoarPower>() || e.HasPower<FlutterPower>()) ?? false;
    
    protected override async Task Play(PlayerChoiceContext context, CardPlay? play) {
        PowerModel? flying = play.Target.GetPower<AirbournePower>();
        flying ??= play.Target.GetPower<SoarPower>();
        flying ??= play.Target.GetPower<FlutterPower>();
        if (flying != null) {
            await DamageCmd.Attack(Amount).FromCard(Card, play).Targeting(play.Target).WithHitFx($"vfx/vfx_attack_blunt").Execute(context);
            await PowerCmd.Remove(flying);
            await PowerCmd.Apply<RingingPower>(context, Card.Owner.Creature, 2, Card.Owner.Creature, Card);
            await CreatureCmd.Stun(play.Target);
        }
    }
}