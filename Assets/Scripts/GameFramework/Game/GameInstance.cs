using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    internal IObjectMap map { get; private set; }

    IPlayer attacker;
    IPlayer defender;

    Queue<Attacker> actionsInProgress;

    public event Action GameOver;

    public bool IsRunning { get; private set; }

    public void Run(IPlayer attacker, IPlayer defender)
    {
        //Resets inner game state
        actionsInProgress.Clear();
        map.ReloadMap();

        //Assigns players
        this.attacker = attacker;
        this.defender = defender;        

        IsRunning = true;
    }

    private void FixedUpdate()
    {
        if (IsRunning)
        {
            int count = actionsInProgress.Count;

            if (count == 0)
                return;

            //Debug.Log(string.Format("Number of running action: {0}", actionsInProgress.Count));
            for (int i = 0; i < count; i++)
            {
                Attacker attacker = actionsInProgress.Dequeue();


                if (attacker.Action != null && attacker.Action.Execute())
                    actionsInProgress.Enqueue(attacker);

            }

            if (defender.ownTroops.Count == 0 || attacker.ownTroops.Count == 0)
                GameOverHandler();
        }
    }

    internal Army GetEnemyArmy(Role role)
    {
        if (role == Role.Attacker)
            return defender.ownTroops;
        else
            return attacker.ownTroops;
    }

    internal Army GetArmy(Role role)
    {
        if (role == Role.Defender)
            return defender.ownTroops;
        else
            return attacker.ownTroops;
    }

    private void GameOverHandler()
    {
        IsRunning = false;
        GameOver?.Invoke();
    }

}
