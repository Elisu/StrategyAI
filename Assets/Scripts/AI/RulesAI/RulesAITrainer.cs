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

    public override Type AIPlayerType => typeof(RulesAI);

    protected override List<AIPlayer> CreatPopulation()
    {
        ICondition damaged = new Conditions.Damaged();
        ICondition free = new Conditions.Free();

        IMacroAction[] actions = new IMacroAction[5] { new SerializableMacroActions.AttackClosest(), 
                                                       new SerializableMacroActions.AttackWithLowestHealth(), 
                                                       new SerializableMacroActions.AttackWithLowestDamage(), 
                                                       new SerializableMacroActions.AttackInRange(), 
                                                       new SerializableMacroActions.DoNothing() };
        ICondition[] conditions = new ICondition[2] { damaged, free };
        all = new Strategy(PopulationSize, IndividualLength, actions, conditions);

        List<AIPlayer> pop = new List<AIPlayer>();

        for (int i = 0; i < all.PopulationSize; i++)
            pop.Add(new RulesAI(all[i], actions));

        return pop;
    }

    public override void GenerationDone()
    {
        int bestFitness = 0;

        for (int i = 0; i < Population.Count; i++)
        {
            all[i].Fitness = ((RulesAI)Population[i]).GetFitnessMean();
            if (bestFitness < all[i].Fitness)
                bestFitness = all[i].Fitness;
        }

        Debug.LogWarning(string.Format("Best individual: {0}", bestFitness));

        all.GeneticOperations(Operators.RouletteWheel, Operators.UniformCrossover, Operators.ActionMutation);
    }

    public override AIPlayer GetRepresentative()
    {
        throw new System.NotImplementedException();
    }

    public override AIPlayer ToSave()
    {
        AIPlayer best = Population[0];

        foreach (AIPlayer player in Population)
            if (((RulesAI)player).GetFitnessMean() > ((RulesAI)best).GetFitnessMean())
                best = player;

        return best;
    }
}
