using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TrainingInstance : Instance
{
    public override bool IsTraining => true;

    int loopsWithoutAction;
    int loops;

    public void Run(AIPlayer attack, AIPlayer defend)
    {
        SetPlayers(attack, defend);
        IsRunning = true;
        loops = 0;
        loopsWithoutAction = 0;

        RunGameTrainingLoop();
    }

    private void RunGameTrainingLoop()
    {
        scheduler.Shopping(this);
        scheduler.Shopping(this);
        scheduler.Shopping(this);

        while (defender.Info.OwnArmy.Troops.Count != 0 && attacker.Info.OwnArmy.Troops.Count != 0)
        {
            if (IsRunning)
            {
                scheduler.Shopping(this);
                scheduler.ScheduleActions();

                if (scheduler.RunningActionsCount == 0)
                    loopsWithoutAction++;
                else
                    loopsWithoutAction = 0;

                scheduler.RunActions();
                loops++;
            }

            if (loopsWithoutAction > 10000 || loops > 100000)
                break;
        }

        GameStats stats;

        if (attacker.Info.OwnArmy.Troops.Count == 0)
            stats = GetGameStats(Role.Defender);
        else if (defender.Info.OwnArmy.Troops.Count == 0)
            stats = GetGameStats(Role.Attacker);
        else
            stats = GetGameStats(Role.Neutral);

        Debug.Log(string.Format("Winner is {0}", stats.Winner));
        ((AIPlayer)defender).RunOver(stats);
        ((AIPlayer)attacker).RunOver(stats);

        //GameOverHandler();
    }

    private GameStats GetGameStats(Role winner)
    {
        //Debug.LogWarning(string.Format("The WINNER is: {0}", winner.ToString()));

        List<Statistics> attackerStats = attacker.Info.OwnArmy.GetAllStats();
        List<Statistics> defenderStats = defender.Info.OwnArmy.GetAllStats();

        return new GameStats(attackerStats, defenderStats, winner); 
    }
}
