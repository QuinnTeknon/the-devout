using Devout.DevoutCode.Cards;
using Devout.DevoutCode.Enchantments;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Debuffs;
using Devout.DevoutCode.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Commands;

public class GeneralCmd {
    public static async Task EnchantCardsInPile(PlayerChoiceContext context, EnchantmentModel enchantment, PileType pile, decimal number, AbstractModel source, bool random = false) {
        Player? player = GetPlayerSource(source);
        IEnumerable<CardModel> cards = [];
        switch (random) {
            case true:
                cards = GetPileFromType(player, pile).Cards.TakeRandom((int) number, player.RunState.Rng.CombatCardGeneration);
                break;
            case false:
                CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, (int) number);
                switch (pile) {
                    case PileType.Hand:
                        cards = await CardSelectCmd.FromHand(context, player, prefs, null, source);
                        break;
                    case PileType.Draw: case PileType.Discard:
                        cards = await CardSelectCmd.FromSimpleGrid(context, (
                                from c in pile.GetPile(player).Cards
                                orderby c.Rarity, c.Id
                                select c).ToList(), player, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, (int) number));
                        break;
                }
                break;
        }
        foreach (CardModel item in cards) {
            Enchant(enchantment.ToMutable(), item);
        }
    }
    
    public static async Task CreateEnchantedCards<T>(EnchantmentModel? enchantment, PileType pile, CardPilePosition position, decimal number = 1, AbstractModel? source = null, Player? player = null) where T : CardModel {
        IReadOnlyList<CardPileAddResult> result = await CreateCards<T>(pile, position, number, source, player);
        List<CardModel> cards = result.Select(c => c.cardAdded).ToList();
        if (enchantment != null) {
            foreach (CardModel item in cards) {
                Enchant(enchantment.ToMutable(), item);
            }
        } else {
            CardCmd.PreviewCardPileAdd(result);
        }
    }

    public static async Task<IReadOnlyList<CardPileAddResult>> CreateCards<T>(PileType pile, CardPilePosition position, decimal number = 1, AbstractModel? source = null, Player? player = null) where T : CardModel {
        player ??= GetPlayerSource(source);
        List<CardModel> cards = CardFactory.GetDistinctForCombat(player, Enumerable.Repeat(0, (int) number).Select(c => ModelDb.Card<T>()).ToList(), (int) number, player?.RunState.Rng.CombatCardGeneration).ToList();
        return await CardPileCmd.AddGeneratedCardsToCombat(cards, pile, player, position);
    }

    public static async Task Enchant(EnchantmentModel? enchantment, CardModel card, decimal amount = 1) {
        CardCmd.Enchant(enchantment.IsMutable ? enchantment : enchantment.ToMutable(), card, amount);
        NCardEnchantVfx nCardEnchantVfx = NCardEnchantVfx.Create(card);
        if (nCardEnchantVfx != null) {
            NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
        }
        await AfterEnchanted(new ThrowingPlayerChoiceContext(), card);
    }

    public static CardPile? GetPileFromType(Player? player, PileType pile) {
        if (player == null) {
            return pile switch {
                PileType.Deck => player.Deck,
                PileType.Draw => player.PlayerCombatState.DrawPile,
                PileType.Discard => player.PlayerCombatState.DiscardPile,
                PileType.Hand => player.PlayerCombatState.Hand,
                PileType.Exhaust => player.PlayerCombatState.ExhaustPile,
            };
        }
        return null;
    }

    public static Player? GetPlayerSource(AbstractModel source) {
        if (source is CardModel card) {
            return card.Owner;
        }
        if (source is PowerModel power) {
            return power.Owner.Player;
        }
        return null;
    }
    
    public static void ReduceCost(CardModel card, int amount) {
        card.EnergyCost.AddThisTurn(-amount);
    }

    public static async Task Lifesteal(PlayerChoiceContext context, Creature? target, Creature leech, decimal number, CardModel? source) {
        await PowerCmd.Apply<LifestealPower>(context, leech, number, target, source, true);
        switch (Math.Sign(number)) {
            case +1: await CreatureCmd.Heal(leech, number); break;
            case -1: await LoseHP(context, leech, Math.Abs(number), source, null); break;
        }
    }
    public static async Task LoseHP(PlayerChoiceContext context, Creature creature, decimal number, CardModel source, CardPlay? play) {
        await CreatureCmd.Damage(context, creature, number, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, source, play);
    }

    public static List<CardModel> GetFlamingCards(Player player, PileType pile) {
        CardPile p = GetPileFromType(player, pile);
        return p.Cards.OfType<Burn>().Select(c => (CardModel) c).Union(p.Cards.Where(c => c.Enchantment is Soulflame)).ToList();
    }
    
    public static List<CardModel> GetEnchantedCards<T>(Player player, PileType pile) where T : EnchantmentModel {
        CardPile p = GetPileFromType(player, pile);
        return p.Cards.Where(c => c.Enchantment is T).ToList();
    }

    public static int GetPowerModifier<T>(Creature creature, int divisor) where T : PowerModel {
        return creature.GetPowerAmount<T>() / divisor;
    }
    
    public static int GetDualPowerModifier<T1, T2>(Creature creature, int divisor) where T1 : PowerModel where T2 : PowerModel {
        return (creature.GetPowerAmount<T1>() + creature.GetPowerAmount<T2>()) / divisor;
    }
    
    private static Task AfterEnchanted(PlayerChoiceContext context, CardModel target) {
        ICombatState state = target.CombatState;
        foreach (AbstractModel model in state.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card) {
                    card.AfterCardEnchanted(context, target);
                } else if (model is DevoutPower power) {
                    power.AfterCardEnchanted(context, target);
                } else if (model is DevoutRelic relic) {
                    relic.AfterCardEnchanted(context, target);
                }
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }

    public static async Task<CardPileAddResult?> TransformTo<T>(CardModel from) where T : CardModel {
        EnchantmentModel? enchantment = from.Enchantment;
        AfflictionModel? affliction = from.Affliction;
        CardPileAddResult? result = await CardCmd.TransformTo<T>(from, CardPreviewStyle.None);
        CardModel? to = result?.cardAdded;
        if (to != null) {
            if (enchantment != null)
                await Enchant(enchantment, to, enchantment.Amount);
            if (affliction != null)
                await CardCmd.Afflict(affliction, to, affliction.Amount);
            return result;
        }
        return null;
    }

    public static CardModel CardFromType(Type? type, out CardModel? card) {
        card = ModelDb.GetByIdOrNull<CardModel>(ModelDb.GetId(type));
        return card;
    }
}