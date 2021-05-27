using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public struct GameStats
{
    public Role Winner { get; private set; }

    private readonly ReadOnlyCollection<Statistics> attackerStats;
    private readonly ReadOnlyCollection<Statistics> defenderStats;

    public GameStats(List<Statistics> attacker, List<Statistics> defender, Role win)
    {
        attackerStats = attacker.AsReadOnly();
        defenderStats = defender.AsReadOnly();
        Winner = win;
    }

    public ReadOnlyCollection<Statistics> GetMyStats(Role myRole)
    {
        if (myRole == Role.Attacker)
            return attackerStats;
        else
            return defenderStats;
    }

    public ReadOnlyCollection<Statistics> GetEnemyStats(Role myRole)
    {
        if (myRole == Role.Defender)
            return attackerStats;
        else
            return defenderStats;
    }

    public GameStats FilterStatistics (Func<Statistics, bool> filter)
    {
        List<Statistics> attacker = new List<Statistics>();
        List<Statistics> defender = new List<Statistics>();

        foreach (Statistics stat in attackerStats)
            if (filter.Invoke(stat))
                attacker.Add(stat);

        foreach (Statistics stat in defenderStats)
            if (filter.Invoke(stat))
                defender.Add(stat);

        return new GameStats(attacker, defender, Winner);
    }
}
