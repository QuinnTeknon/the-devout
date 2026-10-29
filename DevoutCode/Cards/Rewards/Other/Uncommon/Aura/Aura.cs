using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Commands;
using Devout.DevoutCode.Powers.Buffs.Flagellance;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Cards.Rewards.Other.Uncommon.Aura;

public abstract class Aura<C>() : DevoutCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IFlipCard<C> where C : DevoutCard {
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromCard<C>(IsUpgraded), HoverTipFactory.FromPower<FlagellantPower>()];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext context) {
        await Cmd.Wait(0.25f);
        await HP(context);
        await Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade() {
        RemoveKeyword(CardKeyword.Ethereal);
        AddKeyword(CardKeyword.Retain);
    }

    public virtual Task HP(PlayerChoiceContext context) => Task.CompletedTask;
    public virtual Task Effect(PlayerChoiceContext context) => Task.CompletedTask;
}