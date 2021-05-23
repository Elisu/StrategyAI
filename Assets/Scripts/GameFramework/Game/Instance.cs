using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instance : MonoBehaviour
{
    public event Action GameOver;

    protected IPlayer attacker;
    protected IPlayer defender;    

    internal IObjectMap Map { get; private set; }

    private protected Scheduler scheduler = new Scheduler();

    public bool IsRunning { get; protected set; }

    public abstract bool IsTraining { get; }

    public abstract void Run(IPlayer attack, IPlayer defend);

    protected void SetPlayers(IPlayer attack, IPlayer defend)
    {
        attacker = attack;
        defender = defend;

        attacker.Start(this, Role.Attacker);
        defender.Start(this, Role.Defender);

        attacker.OwnArmy.SetEnemy(this);
        defender.OwnArmy.SetEnemy(this);

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
            return defender.OwnArmy;
        else
            return attacker.OwnArmy;
    }

    internal Army GetArmy(Role role)
    {
        if (role == Role.Defender)
            return defender.OwnArmy;
        else
            return attacker.OwnArmy;
    }

    protected void GameOverHandler()
    {
        GameOver?.Invoke();
    }

}
