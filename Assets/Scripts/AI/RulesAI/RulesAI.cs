using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Genetic;
using System.Linq;

[DataContract]
public class RulesAI : AIPlayer
{
    public int Fitness { get; private set; }

    [DataMember]
    readonly StrategyIndividual individual;

    [DataMember]
    readonly ShopperIndividual shopper;

    [DataMember]
    readonly IMacroAction[] possibleActions;

    [DataMember]
    const int lowUnitTreshold = 1;

    ConcurrentQueue<GameStats> accumulatedResults;

    public RulesAI(StrategyIndividual ind, IMacroAction[] actions, ShopperIndividual shopperInd)
    {
        possibleActions = actions;
        individual = ind;
        accumulatedResults = new ConcurrentQueue<GameStats>();
        shopper = shopperInd;
    }

    private RulesAI(StrategyIndividual ind, IMacroAction[] actions, ShopperIndividual shopperInd, ConcurrentQueue<GameStats> fitnesses)
    {
        possibleActions = actions;
        individual = ind;
        accumulatedResults = fitnesses;
        shopper = shopperInd;
        
    }


    public override AIPlayer Clone()
    {
        return new RulesAI(new StrategyIndividual(individual), possibleActions, shopper, accumulatedResults);
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction resultAction = null;
        int[] votes = new int[individual.PossibleActionsCount];

        foreach (Rule rule in individual)
        {
            if (rule.AllTrue(attacker, Info))
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
        return shopper.GetNext(Info.OwnArmy.Money);
    }

    protected override void RunOver(GameStats stats)
    {
        accumulatedResults.Enqueue(stats);
    }

    public void EvaluateFitness(FitnessCalculation fitnessFunction = null)
    {
        GameStats[] stats = accumulatedResults.ToArray();

        if (fitnessFunction != null)
            Fitness = fitnessFunction(stats, Side);
        else
            Fitness = GetMeanFitness(stats, Side);

        individual.AddFitness(Fitness);
        shopper.AddFitness(Fitness);

    }

    private int GetMeanFitness(GameStats[] stats, Role role)
    {
        List<int> fitnesses = new List<int>();

        foreach (GameStats gameStat in stats)
           fitnesses.Add(EvolutionFunctions.ComputeFitness(gameStat, role));

        fitnesses.Sort();
        return fitnesses[fitnesses.Count / 2];
    }

    
}
