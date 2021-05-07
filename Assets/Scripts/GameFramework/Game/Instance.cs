using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instance : MonoBehaviour
{
    public event Action GameOver;

    protected IPlayer attacker;
    protected IPlayer defender;

    [SerializeField]
    GameObject mapObject;

    internal IObjectMap Map { get; private set; }

    private protected Scheduler scheduler = new Scheduler();

    public bool IsRunning { get; protected set; }

    public abstract bool IsTraining { get; }

    public abstract void Run(IPlayer attack, IPlayer defend);

    private void Awake()
    {
        LoadMap();
    }

    protected void SetPlayers(IPlayer attack, IPlayer defend)
    {
        attacker = attack;
        defender = defend;

        attacker.Start(this, Role.Attacker);
        defender.Start(this, Role.Defender);

        attacker.OwnArmy.SetEnemy(this);
        defender.OwnArmy.SetEnemy(this);

        scheduler.Set(attacker, defender);
    }


    public void Restart()
    {
        Map.ReloadMap();

    }

    private void LoadMap()
    {
        List<List<Transform>> mapPrefab = new List<List<Transform>>();

        foreach (Transform row in mapObject.transform)
        {
            List<Transform> fields = new List<Transform>();

            foreach (Transform field in row.gameObject.transform)
                fields.Add(field);

            mapPrefab.Add(fields);
        }

        Map = new IObjectMap(mapPrefab.Count, mapPrefab[0].Count, mapPrefab);
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
