using Devout.DevoutCode.Cards.Rewards.Braid.Templates;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Braid;

public class Braidcrush() : BraidAbstract(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("ConstrictPower", 1m), new("StrengthPerConstrict", 1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<StrengthPower>(), ..base.ExtraHoverTips];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
        await Cmd.Wait(0.25f);
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
        await Cmd.Wait(1);
        await PowerCmd.Apply<BraidboundPower>(context, play.Target, DynamicVars["ConstrictPower"].BaseValue, Owner.Creature, this);
        int num = play.Target.GetPower<BraidboundPower>()?.Amount ?? 0;
        await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, num, Owner.Creature, this);
    }

    protected override void OnUpgrade() {
        DynamicVars["ConstrictPower"].UpgradeValueBy(1m);
    }
}