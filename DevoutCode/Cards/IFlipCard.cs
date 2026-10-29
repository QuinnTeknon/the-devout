using Devout.DevoutCode.Cards.Rewards.Other.Rare.Heart;
using Devout.DevoutCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Devout.DevoutCode.Cards;

public interface IFlipCard<C> where C : DevoutCard {
    public static async Task<CardPileAddResult?> Flip(DevoutCard self) {
        await Cmd.Wait(0.25f);
        await CardPileCmd.Add(self, PileType.Hand);
        var result = await GeneralCmd.TransformTo<C>(self);
        await Cmd.Wait(0.25f);
        return result;
    }
}