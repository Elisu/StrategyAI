using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Genetic;

[DataContract]
public class RulesAI : AIPlayer
{
    [DataMember]
    public Individual individual;

    [DataMember]
    public IMacroAction[] possibleActions;

    ConcurrentQueue<int> AccumulatedFitnesses;

    public RulesAI(Individual ind, IMacroAction[] actions)
    {
        possibleActions = actions;
        individual = ind;
        AccumulatedFitnesses = new ConcurrentQueue<int>();
    }

    private RulesAI(Individual ind, IMacroAction[] actions, ref ConcurrentQueue<int> fitnesses)
    {
        possibleActions = actions;
        individual = ind;
        AccumulatedFitnesses = fitnesses;
    }

    public override AIPlayer Clone()
    {
        return new RulesAI(new Individual(individual), possibleActions, ref AccumulatedFitnesses);
    }

    public int GetFitnessMean()
    {
        int[] fitnessArray = AccumulatedFitnesses.ToArray();

        if (fitnessArray.Length == 1)
            return fitnessArray[0];

        Array.Sort(fitnessArray);
        return fitnessArray[Mathf.CeilToInt(fitnessArray.Length / 2)];
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction resultAction = null;
        int[] votes = new int[individual.PossibleActions];

        foreach (Rule rule in individual)
        {
            if (rule.AllTrue(attacker))
                votes[rule.ActionIndex]++;
        }

        int indexOfBest = 0;

        for (int i = 0; i < votes.Length; i++)
        {
            if (votes[indexOfBest] < votes[i])
                indexOfBest = i;
        }

        possibleActions[indexOfBest].TryAction(attacker, out resultAction);

        return resultAction;
    }

    protected override int PickToBuy()
    {
        return UnitFinder.PickOnBudget(OwnArmy.Money);
    }

    protected override void RunOver(GameStats stats)
    {
        individual.SetFtiness(stats, role);
        AccumulatedFitnesses.Enqueue(individual.Fitness);
    }
}
