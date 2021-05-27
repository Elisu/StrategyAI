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

    IMacroAction[] towerActions;
    IMacroAction[] meleeActions;
    IMacroAction[] rangedActions;

    CoevolutionAI champion;

    public override Type AIPlayerType => typeof(CoevolutionAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        ICondition damaged = new Conditions.Damaged();
        ICondition free = new Conditions.Free();

        IMacroAction attackClosest = new SerializableMacroActions.AttackClosest();
        IMacroAction attackWithLowestHealth = new SerializableMacroActions.AttackWithLowestHealth();
        IMacroAction attackWithLowestDamage = new SerializableMacroActions.AttackWithLowestDamage();
        IMacroAction attackInRange = new SerializableMacroActions.AttackInRange();
        IMacroAction doNothing = new SerializableMacroActions.DoNothing();

        towerActions = new IMacroAction[4] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth };
        ICondition[] conditions = new ICondition[2] { damaged, free };
        towers = new Strategy(PopulationSize, IndividualLength, towerActions, conditions);

        //Melee
        meleeActions = new IMacroAction[5] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing };
        ICondition[] conditions2 = conditions;
        meleeUnits = new Strategy(PopulationSize, IndividualLength, meleeActions, conditions2);

        //Ranged
        rangedActions = new IMacroAction[5] { attackClosest, attackInRange, attackWithLowestDamage, attackWithLowestHealth, doNothing };
        ICondition[] conditions3 = conditions;
        rangedUnits = new Strategy(PopulationSize, IndividualLength, rangedActions, conditions3);

        List<AIPlayer> pop = new List<AIPlayer>();
        return GetPop();
    }

    private List<AIPlayer> GetPop()
    {
        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < towers.PopulationSize; i++)
            for (int j = 0; j < meleeUnits.PopulationSize; j++)
                for (int k = 0; k < rangedUnits.PopulationSize; k++)
                    pop.Add(new CoevolutionAI(towers[i], meleeUnits[j], rangedUnits[k], new List<IMacroAction[]>() {towerActions, rangedActions, meleeActions }));

        return pop;
    }

    public override void GenerationDone()
    {
        foreach (AIPlayer player in population)
            ((CoevolutionAI)player).SetIndividualFitness();

        champion = (CoevolutionAI)FindChampion();
        Debug.LogWarning(string.Format("Coevolution best: {0}", ((CoevolutionAI)GetChampion()).Fitness));

        towers.Evolve();
        meleeUnits.Evolve();
        rangedUnits.Evolve();
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
