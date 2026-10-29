using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Cards.Rewards.Other.Rare.Nukes;

public class Apocalypse() : DevoutCard(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, (X, 0)), IFlipCard<Pandemonium> {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue * piety.CapturedXValue).FromCard(this, play).TargetingAllOpponents(CombatState)
            .WithAttackerAnim("Cast", 0.5f)
            .BeforeDamage(async delegate {
                List<Creature> enemies = CombatState.Enemies.Where(e=> e.IsAlive).ToList();
                NHyperbeamVfx nHyperbeamVfx = NHyperbeamVfx.Create(Owner.Creature, enemies.Last());
                if (nHyperbeamVfx != null) {
                    nHyperbeamVfx.Modulate = Colors.Gold;
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamVfx);
                    await Cmd.Wait(0.5f);
                }
                foreach (Creature target in enemies) {
                    NHyperbeamImpactVfx nHyperbeamImpactVfx = NHyperbeamImpactVfx.Create(Owner.Creature, target);
                    if (nHyperbeamImpactVfx != null) {
                        nHyperbeamImpactVfx.Modulate = Colors.Gold;
                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamImpactVfx);
                    }
                }
            }).Execute(context);
        await PowerCmd.Apply<ForegoneConclusionPower>(context, Owner.Creature, piety.CapturedXValue / DynamicVars.Cards.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}