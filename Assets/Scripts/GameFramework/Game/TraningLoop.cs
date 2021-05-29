using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

internal class TraningLoop : Loop
{
    public AITrainer attacker;  //TrainingSettings.selectedAttacker;
    public AITrainer defender; //TrainingSettings.selectedDefender;

    public int gameInstancesCount = 5;
    public TrainingInstance gameInstance;

    public int GenerationCount;

    private TrainingInstance[] instances;
    private int currentAttacker = 0;
    private int currentDefender = 0;

    private bool trainingFinished = false;
    private bool generationRunning = false;

    private void Start()
    {
        instances = new TrainingInstance[gameInstancesCount];

        attacker = Instantiate(attacker);
        defender = Instantiate(defender);

        //Initializes the AI handlers
        attacker.OnStart();
        defender.OnStart();

        List<List<Transform>> map = LoadMap();
        for (int i = 0; i < gameInstancesCount; i++)
        {
            instances[i] = Instantiate(gameInstance);
            instances[i].SetMap(map);
        }

    }

    private void Update()
    {
        if (trainingFinished)
            return;


        if (GenerationCount <= 0)
        {
            Debug.Log("FINISHED");
            trainingFinished = true;
            attacker.SaveChampion();
            defender.SaveChampion();
            return;
        }

        if (!generationRunning)
            StartCoroutine(RunGeneration());
        //if (RunOneGeneration())
        //{
        //    attacker.GenerationDone();
        //    defender.GenerationDone();
        //    GenerationCount--;

        //    if (GenerationCount > 0)
        //    {
        //        attacker.BeforeEachGeneration();
        //        defender.BeforeEachGeneration();
        //    }
        //}
    }

    IEnumerator RunGeneration()
    {
        generationRunning = true;

        yield return new WaitUntil(PerformOneGeneration);

        attacker.GenerationDone();
        defender.GenerationDone();
        GenerationCount--;

        if (GenerationCount > 0)
        {
            attacker.BeforeEachGeneration();
            defender.BeforeEachGeneration();
        }

        generationRunning = false;
    }


    private bool PerformOneGeneration()
    {
        IList<AIPlayer> attackerPop = attacker.Population;
        IList<AIPlayer> defenderPop = defender.Population;

        currentAttacker = 0;
        currentDefender = 0;
        List<Task> runningTasks = new List<Task>();

        while (currentAttacker != attackerPop.Count || currentDefender != defenderPop.Count)
        {            
            runningTasks.Clear();
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

                if (currentAttacker < attackerPop.Count - 1)
                    currentDefender = 0;
            }

            Task.WaitAll(runningTasks.ToArray());
        }

        return true;
    }

    private bool RunOneGeneration()
    {
        IList<AIPlayer> attackerPop = attacker.Population;
        IList<AIPlayer> defenderPop = defender.Population;

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

            if (currentAttacker < attackerPop.Count - 1)
                currentDefender = 0;
        }

        Task.WaitAll(runningTasks.ToArray());

        if (currentAttacker == attackerPop.Count && currentDefender == defenderPop.Count)
        {
            currentAttacker = 0;
            currentDefender = 0;
            return true;
        }

        return false;
    }    
}
