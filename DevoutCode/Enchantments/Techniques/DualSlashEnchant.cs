using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Enchantments.Techniques.Templates;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.Enchantments.Techniques;

public class DualSlashEnchant : TechniqueEnchant {
    protected override IEnumerable<DynamicVar> CanonicalVars => [..base.CanonicalVars, new RepeatVar(2)];
    protected override async Task Play(PlayerChoiceContext context, CardPlay? play) {
        for (int i = 0; i < DynamicVars.Repeat.BaseValue - 1; i += 1) {
            Angur angur = (Angur) Card;
            TalkCmd.Play(new LocString(locTable, $"{Title.LocEntryKey.Split(".")[0]}.dialogue.next"), Card.Owner.Creature, VfxColor.Gold);
            await PowerCmd.Apply<VigorPower>(context, angur.Owner.Creature, angur.vigor, angur.Owner.Creature, angur, true);
            await angur.Play(context, play);
        }
    }
}