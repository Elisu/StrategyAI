using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameStats
{
    public List<Statistics> AttackerStats { get; private set; }
    public List<Statistics>  DefenderStats { get; private set; }
    public Role Winner { get; private set; }

    public GameStats(List<Statistics> attacker, List<Statistics> defender, Role win)
    {
        AttackerStats = attacker;
        DefenderStats = defender;
        Winner = win;
    }

    public List<Statistics> GetMyStats(Role myRole)
    {
        if (myRole == Role.Attacker)
            return AttackerStats;
        else
            return DefenderStats;
    }

    public List<Statistics> GetEnemyStats(Role myRole)
    {
        if (myRole == Role.Defender)
            return AttackerStats;
        else
            return DefenderStats;
    }
}
