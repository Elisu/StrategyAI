using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instance : MonoBehaviour
{
    internal event Action GameOver;

    protected IPlayer attacker;
    protected IPlayer defender;

    protected int loopsWithoutAction;
    protected int loops;

    private const int moneyGainInterval = 200; 

    internal IObjectMap Map { get; private set; }

    private protected Scheduler scheduler = new Scheduler();

    public bool IsRunning { get; protected set; }

    public abstract bool IsTraining { get; }

   internal void SetPlayers(IPlayer attack, IPlayer defend)
    {
        loopsWithoutAction = 0;
        loops = 0;

        attacker = attack;
        defender = defend;

        Army attackerArmy = new Army(Role.Attacker);
        Army defenderArmy = new Army(Role.Defender);

        attacker.Start(new GameInfo(attackerArmy, defenderArmy, Map), Role.Attacker);
        defender.Start(new GameInfo(defenderArmy, attackerArmy, Map), Role.Defender);

        Map.ReloadMap(this);

        scheduler.Set(attacker, defender);
    }

    internal void SetMap(List<List<Transform>> mapPrefab)
    {
        Map = new IObjectMap(mapPrefab.Count, mapPrefab[0].Count, mapPrefab, mapPrefab[0][0].transform.localScale.x);
    }


    internal Army GetEnemyArmy(Role role)
    {
        if (role == Role.Attacker)
            return defender.Info.OwnArmy;
        else
            return attacker.Info.OwnArmy;
    }

    internal Army GetArmy(Role role)
    {
        if (role == Role.Defender)
            return defender.Info.OwnArmy;
        else
            return attacker.Info.OwnArmy;
    }

    protected void GameOverHandler()
    {
        GameOver?.Invoke();
    }

    protected void OneLoop()
    {
        if (IsRunning)
        {
            if (loops % moneyGainInterval == 0)
                scheduler.MoneyGain();

            scheduler.Shopping(this);
            scheduler.ScheduleActions();

            if (scheduler.RunningActionsCount == 0)
                loopsWithoutAction++;
            else
                loopsWithoutAction = 0;

            scheduler.RunActions();
            loops++;
        }
    }

}
