using Genetic;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class CoevolutionAI : AIPlayer
{
    public int Fitness { get; private set; }

    [DataMember]
    StrategyIndividual towers;
    [DataMember]
    StrategyIndividual meleeUnits;
    [DataMember]
    StrategyIndividual rangedUnits;

    /// <summary>
    /// order Towers, Melee, Ranged
    /// </summary>
    [DataMember]
    readonly List<IMacroAction[]> possibleActions;

    ConcurrentQueue<GameStats> accumulatedResults;

    public CoevolutionAI(StrategyIndividual tw, StrategyIndividual melee, StrategyIndividual ranged, List<IMacroAction[]> actions)
    {
        towers = tw;
        meleeUnits = melee;
        rangedUnits = ranged;
        possibleActions = actions;
        accumulatedResults = new ConcurrentQueue<GameStats>();
    }

    private CoevolutionAI(StrategyIndividual tw, StrategyIndividual melee, StrategyIndividual ranged, List<IMacroAction[]> actions, ConcurrentQueue<GameStats> fitnesses)
    {
        towers = tw;
        meleeUnits = melee;
        rangedUnits = ranged;
        possibleActions = actions;
        accumulatedResults = fitnesses;
    }

    private CoevolutionAI DeepCopy()
    {
        StrategyIndividual tw = new StrategyIndividual(this.towers);
        StrategyIndividual melee = new StrategyIndividual(this.meleeUnits);
        StrategyIndividual ranged = new StrategyIndividual(this.rangedUnits);

        return new CoevolutionAI(tw, melee, ranged, possibleActions, accumulatedResults);
    }

    public override AIPlayer Clone()
    {
        return DeepCopy();
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction resultAction = null;
        IMacroAction[] actions;
        StrategyIndividual current;

        if (attacker is TowerBase)
        {
            current = towers;
            actions = possibleActions[0];
        }           
        else if (attacker is Troop<Archers>)
        {
            current = rangedUnits;
            actions = possibleActions[1];
        }
        else
        {
            current = meleeUnits;
            actions = possibleActions[2];
        }            

        int[] votes = new int[current.PossibleActions];

        foreach (Rule rule in current)
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


        actions[indexOfBest].TryAction(attacker, out resultAction);
        return resultAction;
    }

    protected override void RunOver(GameStats stats)
    {
        accumulatedResults.Enqueue(stats);            
    }

    protected override int PickToBuy()
    {
        return UnitFinder.PickOnBudget(Info.OwnArmy.Money);
    }

    public void SetIndividualFitness()
    {
        //towers.AddFitness(accumulatedResults.ToArray(), Side);
        //meleeUnits.AddFitness(accumulatedResults.ToArray(), Side);
        //rangedUnits.AddFitness(accumulatedResults.ToArray(), Side);

        //Fitness = towers.Fitness;
    }
}
