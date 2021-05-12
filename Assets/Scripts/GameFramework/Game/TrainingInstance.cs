using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TrainingInstance : Instance
{
    public override bool IsTraining => true;

    public override void Run(IPlayer attack, IPlayer defend)
    {
        SetPlayers(attack, defend);
        IsRunning = true;

        RunGameTrainingLoop();
    }

    private void RunGameTrainingLoop()
    {
        scheduler.Shopping(this);
        scheduler.Shopping(this);
        scheduler.Shopping(this);

        while (defender.OwnArmy.Count != 0 && attacker.OwnArmy.Count != 0)
        {
            if (IsRunning)
            {
                scheduler.Shopping(this);
                scheduler.ScheduleActions();
                scheduler.RunActions();                
            }
        }

        GameStats stats;

        if (defender.OwnArmy.Count == 0 && attacker.OwnArmy.Count == 0)
            stats = GetGameStats(Role.Neutral);
        else if (attacker.OwnArmy.Count == 0)
            stats = GetGameStats(Role.Defender);
        else
            stats = GetGameStats(Role.Attacker);
        
        defender.RunOver(stats);
        attacker.RunOver(stats);

        GameOverHandler();
    }

    private GameStats GetGameStats(Role winner)
    {
        //Debug.LogWarning(string.Format("The WINNER is: {0}", winner.ToString()));

        List<Statistics> attackerStats = attacker.OwnArmy.GetAllStats();
        List<Statistics> defenderStats = defender.OwnArmy.GetAllStats();

        return new GameStats(attackerStats, defenderStats, winner); 
    }
}
