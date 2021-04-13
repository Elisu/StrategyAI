using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    internal static IObjectMap map;
    internal static Army defenderArmy;
    internal static Army attackerArmy;
    internal static Queue<IAttack> actionsInProgress;

    public static bool IsTrainingMode;

    public static event Action GameOver;

    // Start is called before the first frame update
    void Awake()
    {
        defenderArmy = new Army(Role.Defender); 
        attackerArmy = new Army(Role.Attacker);
        defenderArmy.SetEnemy();
        attackerArmy.SetEnemy();
        actionsInProgress = new Queue<IAttack>();
    }

    void FixedUpdate()
    {
        int count = actionsInProgress.Count;
        Debug.Log(string.Format("Number of running action: {0}", actionsInProgress.Count));
        for (int i = 0; i < count; i++)
        {
            IAttack action = actionsInProgress.Dequeue();


            if (action != null && action.Action.Execute())
                actionsInProgress.Enqueue(action);

        }

        if (defenderArmy.Count == 0 || attackerArmy.Count == 0)
            GameOverHandler();
            
    }

    internal static Army GetEnemyArmy(Role role)
    {
        if (role == Role.Attacker)
            return defenderArmy;
        else
            return attackerArmy;
    }

    internal static Army GetArmy(Role role)
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
