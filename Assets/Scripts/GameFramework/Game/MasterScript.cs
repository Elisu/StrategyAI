using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    [SerializeField]
    private TraningLoop trainer;

    //internal static IObjectMap map;
    internal static Army defenderArmy;
    internal static Army attackerArmy;
    internal static Queue<Attacker> actionsInProgress;

    public static bool IsTrainingMode;

    public static event Action GameOver;

    // Start is called before the first frame update
    void Awake()
    {
        defenderArmy = new Army(Role.Defender); 
        attackerArmy = new Army(Role.Attacker);
        defenderArmy.SetEnemy();
        attackerArmy.SetEnemy();
        actionsInProgress = new Queue<Attacker>();
    }

    void FixedUpdate()
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
        //map.ReloadMap();

        GameOver?.Invoke();
        trainer.Run();
    }    
}
