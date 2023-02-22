using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TrainingInstance : Instance
{
    public override bool IsTraining => true;

    int generationCount = 0;

    private int loopLimit = 10000;

    public void Run(AIPlayer attack, AIPlayer defend, int genCount)
    {
        SetPlayers(attack, defend);
        IsRunning = true;
        generationCount = genCount;

        RunGameTrainingLoop();
    }

    private void RunGameTrainingLoop()
    {
        scheduler.Shopping(this);
        scheduler.Shopping(this);
        scheduler.Shopping(this);

        while (defender.Info.OwnArmy.Troops.Count != 0 && attacker.Info.OwnArmy.Troops.Count != 0)
        {
            OneLoop();

            if (loopsWithoutAction > 1000 || loops >= loopLimit)
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
        Debug.Log(string.Format("Gen {0}: Winner is {1}", generationCount, stats.Winner));
        ((AIPlayer)defender).RunOver(stats);
        ((AIPlayer)attacker).RunOver(stats);

        //GameOverHandler();
    }

    private GameStats GetGameStats(Role winner)
    {
        //Debug.LogWarning(string.Format("The WINNER is: {0}", winner.ToString()));

        List<Statistics> attackerStats = attacker.Info.OwnArmy.GetAllStats();
        List<Statistics> defenderStats = defender.Info.OwnArmy.GetAllStats();

        return new GameStats(attackerStats, defenderStats, loopLimit - loops, winner); 
    }
}
