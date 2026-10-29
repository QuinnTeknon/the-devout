using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using Devout.DevoutCode.Cards.Rewards.Angur.Templates;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.Powers.Buffs;

public class GigantesPower : DevoutPower {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DisplayVar<GigantesPower>("DamageCalculation", p => ((p.Base - p.Addition) + p.Addition * p.Amount).ToString())];
    public override int DisplayAmount => (int) DynamicVars["DamageCalculation"].BaseValue;

    public int Base = 0;
    public int Addition = 0;

    public override async Task AfterAngurUnsheathed(PlayerChoiceContext context, Cards.Tokens.Angurvadal.Angur angur) {
        if (angur.Owner == Owner.Player) {
            Child child = await PowerCmd.Apply<Child>(context, Owner, 1, null, null);
            child.DynamicVars["DamageModifier"].UpgradeValueBy(DynamicVars["DamageCalculation"].BaseValue);
        }
    }
    
    public class Child : DevoutPower {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new("DamageModifier", 0)];
        
        public string gigantes = LocString.GetIfExists("powers", $"DEVOUT-{typeof(GigantesPower).Name.ToSnakeCase().ToUpper()}.description").GetFormattedText().Split(", ")[1];
        
        public override List<(string, string)> Localization => LocManager.Instance.Language switch { _ => new CardLoc(
            "Gargant Soul",
            gigantes[0].ToString().ToUpper() + gigantes.Substring(1)
        )};

        public override string CustomPackedIconPath { get; }

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? source, CardPlay? play) {
            if (!props.IsPoweredAttack())
                return 1m;
            if (dealer == null)
                return 1m;
            if (target == dealer)
                return 1m;
            if (!(target == Owner || dealer == Owner))
                return 1m;
            return 100 + DynamicVars["DamageModifier"].BaseValue / 100m * (dealer == Owner ? +1 : -1);
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants) {
            if (side == CombatSide.Enemy)
                await PowerCmd.Remove(this);
        }
    }
}