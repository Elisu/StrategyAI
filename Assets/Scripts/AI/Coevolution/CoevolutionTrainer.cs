using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetic;
using System;

public class CoevolutionTrainer : AITrainer
{
    public int PopulationSize;
    public int IndividualLength;

    Strategy towers;
    Strategy meleeUnits;
    Strategy rangedUnits;

    ShopperPopulation shopperStrategy;

    IMacroAction[] towerActions;
    IMacroAction[] meleeActions;
    IMacroAction[] rangedActions;

    CoevolutionAI champion;

    public override Type AIPlayerType => typeof(CoevolutionAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        ICondition damaged = new Conditions.Damaged();
        ICondition free = new Conditions.Free();
        ICondition strongest = new Conditions.Strongest();
        ICondition clostestIsTroop = new Conditions.ClosestIsTroopBase();

        IMacroAction attackClosest = new SerializableMacroActions.AttackClosest();
        IMacroAction attackWithLowestHealth = new SerializableMacroActions.AttackWithLowestHealth();
        IMacroAction attackWithLowestDamage = new SerializableMacroActions.AttackWithLowestDamage();
        IMacroAction attackInRange = new SerializableMacroActions.AttackInRange();
        IMacroAction doNothing = new SerializableMacroActions.DoNothing();
        IMacroAction attackWeakest = new SerializableMacroActions.AttackWeakestAgainstMe();

        towerActions = new IMacroAction[4] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth };
        ICondition[] conditions = new ICondition[2] { damaged, free};
        towers = new Strategy(PopulationSize, IndividualLength, towerActions.Length, conditions);

        //Melee
        meleeActions = new IMacroAction[6] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing, attackWeakest };
        ICondition[] conditions2 = new ICondition[4] { damaged, free, strongest, clostestIsTroop };
        meleeUnits = new Strategy(PopulationSize, IndividualLength, meleeActions.Length, conditions2);

        //Ranged
        rangedActions = new IMacroAction[6] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing, attackWeakest };
        ICondition[] conditions3 = conditions2;
        rangedUnits = new Strategy(PopulationSize, IndividualLength, rangedActions.Length, conditions3);

        //Shopper
        shopperStrategy = new ShopperPopulation(PopulationSize);

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
            ((CoevolutionAI)player).EvaluateFitness();

        champion = (CoevolutionAI)FindChampion();
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
}
