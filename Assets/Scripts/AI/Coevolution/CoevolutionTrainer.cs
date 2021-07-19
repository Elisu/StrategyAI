using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetic;
using System;
using System.IO;
using System.Globalization;

public class CoevolutionTrainer : AITrainer
{
    public int PopulationSize;
    public int IndividualLength;

    public float crossoverProbability;
    public float mutationProbability;
    public float actionMutationProbability = 0.1f;

    Strategy towers;
    Strategy meleeUnits;
    Strategy rangedUnits;

    ShopperPopulation shopperStrategy;

    IMacroAction[] towerActions;
    IMacroAction[] meleeActions;
    IMacroAction[] rangedActions;

    CoevolutionAI champion;

    readonly List<int> fitnessesRecord = new List<int>();
    readonly List<Tuple<int, int, int, int, Role>> accumulatedStats = new List<Tuple<int, int, int, int, Role>>();

    public override Type AIPlayerType => typeof(CoevolutionAI);

    protected override List<AIPlayer> CreatePopulation()
    {
        ICondition damaged = new Conditions.Damaged();
        ICondition free = new Conditions.Free();
        ICondition strongest = new Conditions.StrongerThanClosest();
        ICondition clostestIsTroop = new Conditions.ClosestIsTroopBase();
        ICondition closestIsBuilding = new Conditions.ClosestIsBuilding();
        ICondition closestIsTower = new Conditions.ClosestIsTower();
        ICondition isDefender = new Conditions.IsDefender();
        ICondition healthier = new Conditions.HealthierThanClosest();
        ICondition alone = new Conditions.IsAlone();
        ICondition winning = new Conditions.IsWinning();
        ICondition insideCastle = new Conditions.IsInsideCastle();
        ICondition inTowerRange = new Conditions.IsInTowerRange(); 

        IMacroAction attackClosest = new SerializableMacroActions.AttackClosest();
        IMacroAction attackWithLowestHealth = new SerializableMacroActions.AttackWithLowestHealth();
        IMacroAction attackWithLowestDamage = new SerializableMacroActions.AttackWithLowestDamage();
        IMacroAction attackInRange = new SerializableMacroActions.AttackInRange();
        IMacroAction doNothing = new SerializableMacroActions.DoNothing();
        IMacroAction attackWeakest = new SerializableMacroActions.AttackWeakestAgainstMe();
        IMacroAction moveToSafety = new SerializableMacroActions.MoveToSafety();

        towerActions = new IMacroAction[4] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth };
        ICondition[] conditions = new ICondition[3] { damaged, free, winning };
        towers = new Strategy(PopulationSize, IndividualLength, towerActions.Length, conditions, crossoverProbability, mutationProbability, actionMutationProbability);

        //Melee
        meleeActions = new IMacroAction[6] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing, attackWeakest };
        ICondition[] conditions2 = new ICondition[12] { damaged, free, strongest, clostestIsTroop, closestIsBuilding, closestIsTower, isDefender, healthier, alone, winning, insideCastle, inTowerRange };
        meleeUnits = new Strategy(PopulationSize, IndividualLength, meleeActions.Length, conditions2, crossoverProbability, mutationProbability, actionMutationProbability);

        //Ranged
        rangedActions = new IMacroAction[7] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing, attackWeakest, moveToSafety };
        ICondition[] conditions3 = conditions2;
        rangedUnits = new Strategy(PopulationSize, IndividualLength, rangedActions.Length, conditions3, crossoverProbability, mutationProbability, actionMutationProbability);

        //Shopper
        shopperStrategy = new ShopperPopulation(PopulationSize, crossoverProbability, mutationProbability);

        List<AIPlayer> pop = new List<AIPlayer>();
        return GetPop();
    }

    private List<AIPlayer> GetPop()
    {
        List<AIPlayer> pop = new List<AIPlayer>();

        var actionList = new List<IMacroAction[]>() { towerActions, rangedActions, meleeActions };

        for (int i = 0; i < towers.PopulationSize; i++)
        {
            var randomMelee = meleeUnits[UnityEngine.Random.Range(0, meleeUnits.PopulationSize)];
            var randomRanged = rangedUnits[UnityEngine.Random.Range(0, rangedUnits.PopulationSize)];
            pop.Add(new CoevolutionAI(towers[i], randomMelee, randomRanged, shopperStrategy[i], actionList));
        }

        for (int i = 0; i < meleeUnits.PopulationSize; i++)
        {
            var randomTower = towers[UnityEngine.Random.Range(0, towers.PopulationSize)];
            var randomRanged = rangedUnits[UnityEngine.Random.Range(0, rangedUnits.PopulationSize)];
            pop.Add(new CoevolutionAI(randomTower, meleeUnits[i], randomRanged, shopperStrategy[i], actionList));
        }

        for (int i = 0; i < rangedUnits.PopulationSize; i++)
        {
            var randomTower = towers[UnityEngine.Random.Range(0, towers.PopulationSize)];
            var randomMelee = meleeUnits[UnityEngine.Random.Range(0, meleeUnits.PopulationSize)];
            pop.Add(new CoevolutionAI(randomTower, randomMelee, rangedUnits[i], shopperStrategy[i], actionList));
        }
        return pop;
    }


    public override void GenerationDone()
    {
        foreach (AIPlayer player in population)
        {
            ((CoevolutionAI)player).EvaluateFitness();
            accumulatedStats.AddRange(((CoevolutionAI)player).FitnessStats);
        }

        accumulatedStats.Add(new Tuple<int, int, int, int, Role>(-1, -1, -1, -1, Role.Neutral));

        champion = (CoevolutionAI)FindChampion();

        fitnessesRecord.Add(champion.Fitness);
        Debug.LogWarning(string.Format("Coevolution best: {0}", ((CoevolutionAI)GetChampion()).Fitness));

        towers.Evolve();
        meleeUnits.Evolve();
        rangedUnits.Evolve();
        shopperStrategy.Evolve();

        population = GetPop();
    }

    private AIPlayer FindChampion()
    {
        AIPlayer best = population[0];

        foreach (AIPlayer player in population)
            if (((CoevolutionAI)player).Fitness > ((CoevolutionAI)best).Fitness)
                best = player;

        return best;
    }
    public override AIPlayer GetChampion() => champion;

    protected override void TrainingFinished()
    {
        using (var stream = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.dataPath), this.name + "-bestRuns-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture))))
        {
            foreach (int fitness in fitnessesRecord)
                stream.WriteLine(fitness);
        }

        using (var stream = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.dataPath), this.name + "-accumulatedStats-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture))))
        {
            int gen = 1;

            foreach (var stat in accumulatedStats)
            {
                if (stat.Item1 != -1)
                    stream.WriteLine("Gen: {0} - {1} {2} {3} {4} {5}", gen, stat.Item1, stat.Item2, stat.Item3, stat.Item4, stat.Item5);
                else
                    gen++;
            }
        }

        fitnessesRecord.Clear();
        accumulatedStats.Clear();
    }
}
