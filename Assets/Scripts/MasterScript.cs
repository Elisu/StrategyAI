using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public static IObjectMap map;
    public static Army defenderArmy = new Army(Role.Defender);
    public static Army attackerArmy = new Army(Role.Attacker);
    public static Queue<IAction> actionsInProgress = new Queue<IAction>();

    // Start is called before the first frame update
    void Start()
    {

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
}
