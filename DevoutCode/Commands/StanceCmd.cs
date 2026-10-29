using Devout.DevoutCode.Cards.Angurvadal;
using Devout.DevoutCode.Cards.Tokens.Angurvadal;
using Devout.DevoutCode.Powers;
using Devout.DevoutCode.Powers.Buffs.Stances;
using Devout.DevoutCode.Powers.Buffs.Stances.Caoineag;
using Devout.DevoutCode.Powers.Buffs.Stances.Cliodna;
using Devout.DevoutCode.Powers.Buffs.Stances.Sifreyja;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devout.DevoutCode.Commands;

public static class StanceCmd {
    public enum Stance {
        CLIODNA, CAOINEAG, NEUTRAL, SIFREYJA
    }

    public static async Task Switch(PlayerChoiceContext context, Creature creature, Stance into, CardModel source, int bonus = 0) {
        switch (into) {
            case Stance.CLIODNA: {
                await PowerCmd.Remove<CaoineagPower>(creature);
                int m = creature.GetPower<CliodnaPower>()?.magnitude ?? 0;
                if (m == 0)
                    await PowerCmd.Apply<CliodnaPower>(context, creature, 1, creature, source);
                await PowerCmd.Apply<CliodnaStrengthPower>(context, creature, bonus + m, creature, source);
                creature.GetPower<CliodnaPower>()?.AfterEntered();
                break;
            }
            case Stance.CAOINEAG: {
                await PowerCmd.Remove<CliodnaPower>(creature);
                int m = creature.GetPower<CaoineagPower>()?.magnitude ?? 0;
                if (m == 0)
                    await PowerCmd.Apply<CaoineagPower>(context, creature, 1, creature, source);
                await PowerCmd.Apply<CaoineagThornsPower>(context, creature, bonus + m, creature, source);
                creature.GetPower<CaoineagPower>()?.AfterEntered();
                break;
            }
            case Stance.SIFREYJA: {
                int m = creature.GetPower<CliodnaPower>()?.magnitude ?? 0;
                m += creature.GetPower<CaoineagPower>()?.magnitude ?? 0;
                await PowerCmd.Remove<CliodnaPower>(creature);
                await PowerCmd.Remove<CaoineagPower>(creature);
                if (m == 0)
                    await PowerCmd.Apply<SifreyjaPower>(context, creature, 1, creature, source);
                await PowerCmd.Apply<SifreyjaSoulPower>(context, creature, bonus + m, creature, source);
                creature.GetPower<SifreyjaPower>()?.AfterEntered();
                break;
            }
            case Stance.NEUTRAL:
                await PowerCmd.Remove<CliodnaPower>(creature);
                await PowerCmd.Remove<CaoineagPower>(creature);
                await PowerCmd.Remove<SifreyjaPower>(creature);
                break;
        }
    }
}