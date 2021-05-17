using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

internal class TraningLoop : MonoBehaviour
{
    public AITrainer attacker;  //TrainingSettings.selectedAttacker;
    public AITrainer defender; //TrainingSettings.selectedDefender;

    public int gameInstancesCount = 5;
    public TrainingInstance gameInstance;

    public int GenerationCount;

    private TrainingInstance[] instances;
    private int currentAttacker = 0;
    private int currentDefender = 0;

    int run = 0;

    private void Start()
    {
        instances = new TrainingInstance[gameInstancesCount];

        attacker = Instantiate(attacker);
        defender = Instantiate(defender);

        //Initializes the AI handlers
        attacker.OnStart();
        defender.OnStart();

        for (int i = 0; i < gameInstancesCount; i++)
            instances[i] = Instantiate(gameInstance);
    }

    private void Update()
    {
        if (GenerationCount <= 0)
        {
            Debug.Log("Traning Finished");
            return;
        }


        if (RunOneGeneration())
        {
            attacker.GenerationDone();
            defender.GenerationDone();
            GenerationCount--;

            if (GenerationCount > 0)
            {
                attacker.BeforeEachGeneration();
                defender.BeforeEachGeneration();
            }

        }


        //Debug.Log("Done");

        foreach (TrainingInstance ins in instances)
            ins.Restart();
    }

    private bool RunOneGeneration()
    {
        List<AIPlayer> attackerPop = attacker.Population;
        List<AIPlayer> defenderPop = defender.Population;

        List<Task> runningTasks = new List<Task>();

        int i = 0;

        for (; currentAttacker < attackerPop.Count; currentAttacker++)
        {
            for (; currentDefender < defenderPop.Count; currentDefender++, i++)
            {
                if (i >= gameInstancesCount)
                    break;

                int index = i;
                AIPlayer att = attackerPop[currentAttacker].Clone();
                AIPlayer def = defenderPop[currentDefender].Clone();

                runningTasks.Add(Task.Run(() => instances[index].Run(att, def)));
            }

            if (i >= gameInstancesCount)
                break;
        }


        //Debug.Log("Waiting");
        Task.WaitAll(runningTasks.ToArray());
        //Debug.Log("Finished Waiting");

        if (currentAttacker == attackerPop.Count && currentDefender == defenderPop.Count)
        {
            currentAttacker = 0;
            currentDefender = 0;
            return true;
        }

        return false;
    }
}
