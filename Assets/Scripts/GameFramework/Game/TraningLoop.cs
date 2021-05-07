using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

internal class TraningLoop : MonoBehaviour
{
    public AITrainingHandler attacker;
    public AITrainingHandler defender;

    public int gameInstancesCount;
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

        for (int i = 0; i < gameInstancesCount; i++)
            instances[i] = Instantiate(gameInstance);
    }

    private void Update()
    {
        if (GenerationCount <= 0)
            return;

        if (RunOneGeneration())
            GenerationCount--;        

        Debug.Log("Done");

        foreach (TrainingInstance ins in instances)
            ins.Restart();
    }

    private bool RunOneGeneration()
    {
        List<AIPlayer> attackerPop = attacker.GetPopulation();
        List<AIPlayer> defenderPop = defender.GetPopulation();

        List<Task> runningTasks = new List<Task>();

        int i = 0;

        for (int j = currentAttacker; j < attackerPop.Count; j++)
            for (int k = currentDefender; k < defenderPop.Count; k++, i++)
            {
                currentAttacker = j;
                currentDefender = k;

                if (i >= gameInstancesCount)
                    break;

                int index = i;
                AIPlayer att = attackerPop[j].Clone();
                AIPlayer def = defenderPop[k].Clone();

                runningTasks.Add(Task.Run(() => instances[index].Run(att, def)));                                 
            }

        Debug.Log("Waiting");
        Task.WaitAll(runningTasks.ToArray());
        Debug.Log("Finished Waiting");

        if (currentAttacker == attackerPop.Count - 1 && currentDefender == defenderPop.Count - 1)
        {
            currentAttacker = 0;
            currentDefender = 0;
            return true;
        }

        return false;
    }
}
