using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public static IObjectMap map;
    public static Army defenderArmy;
    public static Army attackerArmy;
    public static Queue<IAction> actionsInProgress;

    public static bool IsTrainingMode;

    public static event Action GameOver;

    // Start is called before the first frame update
    void Awake()
    {
        defenderArmy = new Army(Role.Defender); 
        attackerArmy = new Army(Role.Attacker);
        actionsInProgress = new Queue<IAction>();
    }

    void FixedUpdate()
    {
        int count = actionsInProgress.Count;
        Debug.Log(string.Format("Number of running action: {0}", actionsInProgress.Count));
        for (int i = 0; i < count; i++)
        {
            IAction action = actionsInProgress.Dequeue();

            if (action.Execute())
                actionsInProgress.Enqueue(action);

        }

        if (defenderArmy.Count == 0 || attackerArmy.Count == 0)
            GameOverHandler();
            
    }

    public static Army GetEnemyArmy(Role role)
    {
        if (role == Role.Attacker)
            return defenderArmy;
        else
            return attackerArmy;
    }

    public static Army GetArmy(Role role)
    {
        if (role == Role.Defender)
            return defenderArmy;
        else
            return attackerArmy;
    }

    private void GameOverHandler()
    {
        //Reset environment
        actionsInProgress.Clear();
        attackerArmy.Clear();
        defenderArmy.Clear();
        map.ReloadMap();

        GameOver?.Invoke();
    }    
}
