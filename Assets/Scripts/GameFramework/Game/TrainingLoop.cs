using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

internal class TrainingLoop : Loop
{
    public TrainingInstance gameInstance;
    public int parallelInstancesCount = 15;

    public bool debugMode = false;

    public bool TrainingInProgress { get; private set; } = false;

    AIController attacker;  //TrainingSettings.selectedAttacker;
    AIController defender; //TrainingSettings.selectedDefender;

    [SerializeField]
    LoadedAI loadedAI;

    TrainingProgressBar progresBar;

    int tries;
    int generationCount;

    TrainingInstance[] instances;
    int currentAttacker;
    int currentDefender;  

    Thread trainingThread;
    bool terminateThread = false;

    int currentGeneration;

    List<List<Transform>> map;

    private void Awake()
    {
        map = LoadMap();
        instances = new TrainingInstance[parallelInstancesCount];
        for (int i = 0; i < instances.Length; i++)
        {
            instances[i] = Instantiate(gameInstance);
            instances[i].SetMap(map);
        }

        progresBar = FindObjectOfType<TrainingProgressBar>();
    }

    public void StartTraining(AIController attack, AIController defend, int tryCount, int genCount, string attackSave = null, string defendSave = null)
    {        
        tries = tryCount;
        generationCount = genCount;

        currentGeneration = 1;
        currentAttacker = 0;
        currentDefender = 0;

        if (debugMode)
        {
            attacker = TrainingLoop.InstantiateController(attackSave, attack, loadedAI);
            defender = TrainingLoop.InstantiateController(defendSave, defend, loadedAI);
        }
        else
        {
            attacker = attack;
            defender = defend;
        }    
       

        //Initializes the AI handlers
        attacker.OnStart();
        defender.OnStart();

        progresBar.SetGenerationCount(generationCount);

        TrainingInProgress = true;

        trainingThread = new Thread(new ThreadStart(PerformOneGeneration));
        trainingThread.IsBackground = true;
        trainingThread.Start();

    }

    private void Update()
    {
        if (!TrainingInProgress)
            return;

        if (generationCount <= 0)
        {
            Debug.Log("FINISHED");
            TrainingInProgress = false;

            if (attacker is AITrainer trainable)
            {
                trainable.Save();
                trainable.TrainingFinished();
            }
               

            if (defender is AITrainer trainable2)
            {
                trainable2.Save();
                trainable2.TrainingFinished();
            }

            Finished();
            return;
        }

        if (!trainingThread.IsAlive)
        {
            if (attacker is AITrainer trainable)
                trainable.GenerationDone();

            if (defender is AITrainer trainable2)
                trainable2.GenerationDone();

            generationCount--;
            currentGeneration++;
            progresBar.SetProgress(generationCount);

            if (generationCount > 0)
            {
                attacker.BeforeEachGeneration();
                defender.BeforeEachGeneration();

                trainingThread = new Thread(new ThreadStart(PerformOneGeneration));
                trainingThread.IsBackground = true;
                trainingThread.Start();
            }
            
        }
    }

    //IEnumerator RunGeneration()
    //{
    //    generationRunning = true;

    //    yield return new WaitUntil(PerformOneGeneration);

    //    attacker.GenerationDone();
    //    defender.GenerationDone();
    //    GenerationCount--;

    //    if (GenerationCount > 0)
    //    {
    //        attacker.BeforeEachGeneration();
    //        defender.BeforeEachGeneration();
    //    }

    //    generationRunning = false;
    //}

    private void OnDestroy()
    {
        terminateThread = true;
    }

    private void PerformOneGeneration()
    {
        IList<AIPlayer> attackerPop = attacker.Population;
        IList<AIPlayer> defenderPop = defender.Population;

        for (int run = 0; run < tries; run++)
        {
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
                        if (i >= instances.Length)
                            break;

                        if (terminateThread)
                            return;

                        int index = i;
                        attackerPop[currentAttacker].Start(null, Role.Attacker);
                        defenderPop[currentDefender].Start(null, Role.Defender);
                        AIPlayer att = attackerPop[currentAttacker].Clone();
                        AIPlayer def = defenderPop[currentDefender].Clone();

                        runningTasks.Add(Task.Run(() => instances[index].Run(att, def, currentGeneration)));
                    }

                    if (i >= instances.Length)
                        break;

                    if (currentAttacker < attackerPop.Count - 1)
                        currentDefender = 0;
                }

                Task.WaitAll(runningTasks.ToArray());
            }
        }
        

        return;
    }

    private void Finished()
    {
        Destroy(attacker);
        Destroy(defender);
    }

    internal static AIController InstantiateController(string AIFile, AIController controller, LoadedAI ld)
    {
        if (!string.IsNullOrEmpty(AIFile))
        {
            AIPlayer player = (AIPlayer)controller.Load(AIFile);
            LoadedAI loadedController = Instantiate(ld);
            loadedController.SetPlayer(player);
            return loadedController;
        }
        else
            return Instantiate(controller);        
    }

    //private bool RunOneGeneration()
    //{
    //    IList<AIPlayer> attackerPop = attacker.Population;
    //    IList<AIPlayer> defenderPop = defender.Population;

    //    List<Task> runningTasks = new List<Task>();

    //    int i = 0;

    //    for (; currentAttacker < attackerPop.Count; currentAttacker++)
    //    {
    //        for (; currentDefender < defenderPop.Count; currentDefender++, i++)
    //        {
    //            if (i >= gameInstancesCount)
    //                break;

    //            int index = i;
    //            AIPlayer att = attackerPop[currentAttacker].Clone();
    //            AIPlayer def = defenderPop[currentDefender].Clone();

    //            runningTasks.Add(Task.Run(() => instances[index].Run(att, def)));
    //        }

    //        if (i >= gameInstancesCount)
    //            break;

    //        if (currentAttacker < attackerPop.Count - 1)
    //            currentDefender = 0;
    //    }

    //    Task.WaitAll(runningTasks.ToArray());

    //    if (currentAttacker == attackerPop.Count && currentDefender == defenderPop.Count)
    //    {
    //        currentAttacker = 0;
    //        currentDefender = 0;
    //        return true;
    //    }

    //    return false;
    //}    
}
