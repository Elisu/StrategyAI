using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetic;
using System;

public class RulesAITrainer : AITrainer
{
    public int PopulationSize;
    public int IndividualLength;

    Strategy all;
    ShopperPopulation shoppers;
    IMacroAction[] possibleActions;

    AIPlayer champion;

    public override Type AIPlayerType => typeof(RulesAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        ICondition damaged = new Conditions.Damaged();
        ICondition free = new Conditions.Free();
        ICondition strongest = new Conditions.Strongest();
        ICondition closestIsTroop = new Conditions.ClosestIsTroopBase();
        ICondition[] conditions = new ICondition[4] { damaged, free, strongest, closestIsTroop };

        possibleActions = new IMacroAction[5] { new SerializableMacroActions.AttackClosest(), 
                                                new SerializableMacroActions.AttackWithLowestHealth(), 
                                                new SerializableMacroActions.AttackWithLowestDamage(), 
                                                new SerializableMacroActions.AttackInRange(), 
                                                new SerializableMacroActions.DoNothing() };

        all = new Strategy(PopulationSize, IndividualLength, possibleActions.Length, conditions);

        shoppers = new ShopperPopulation(PopulationSize);

        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < all.PopulationSize; i++)
            pop.Add(new RulesAI(all[i], possibleActions, shoppers[i]));

        return pop;
    }

    public override void GenerationDone()
    {
        foreach (AIPlayer player in population)
            ((RulesAI)player).EvaluateFitness();

        champion = FindChampion();

        population.Clear();
 
        all.Evolve();
        shoppers.Evolve();

        for (int i = 0; i < all.PopulationSize; i++)
            population.Add(new RulesAI(all[i], possibleActions, shoppers[i]));

        Debug.LogWarning(string.Format("Best fitness: {0}", all.Champion.Fitness));
    }

    private AIPlayer FindChampion()
    {
        AIPlayer best = population[0];

        foreach (AIPlayer player in population)
            if (((RulesAI)player).Fitness > ((RulesAI)best).Fitness)
                best = player;

        return best;
    }
    public override AIPlayer GetChampion() => champion;
}
