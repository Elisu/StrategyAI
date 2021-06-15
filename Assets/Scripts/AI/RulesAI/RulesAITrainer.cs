using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetic;
using System;
using System.IO;

public class RulesAITrainer : AITrainer
{
    public int PopulationSize;
    public int IndividualLength;

    Strategy all;
    ShopperPopulation shoppers;
    IMacroAction[] possibleActions;

    RulesAI champion;
    readonly List<int> fitnessesRecord = new List<int>();

    public override Type AIPlayerType => typeof(RulesAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        ICondition[] conditions = new ICondition[7] { new Conditions.Damaged(),
                                                      new Conditions.Free(),
                                                      new Conditions.Strongest(),
                                                      new Conditions.ClosestIsTroopBase(),
                                                      new Conditions.ClosestIsBuilding(),
                                                      new Conditions.ClosestIsTower(),
                                                      new Conditions.IsDefender() };

        possibleActions = new IMacroAction[6] { new SerializableMacroActions.AttackClosest(), 
                                                new SerializableMacroActions.AttackWithLowestHealth(), 
                                                new SerializableMacroActions.AttackWithLowestDamage(), 
                                                new SerializableMacroActions.AttackWeakestAgainstMe(),
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

        champion = (RulesAI)FindChampion();

        population.Clear();
 
        all.Evolve();
        shoppers.Evolve();

        for (int i = 0; i < all.PopulationSize; i++)
            population.Add(new RulesAI(all[i], possibleActions, shoppers[i]));

        fitnessesRecord.Add(champion.Fitness);
        Debug.LogWarning(string.Format("Best fitness: {0}", champion.Fitness));
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

    protected override void TrainingFinished()
    {
        using (var stream = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.dataPath), "bestRuns")))
        {
            foreach (int fitness in fitnessesRecord)
                stream.WriteLine(fitness);
        }
    }
}
