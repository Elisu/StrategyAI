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
    [DataMember]
    Individual towers;
    [DataMember]
    Individual meleeUnits;
    [DataMember]
    Individual rangedUnits;

    /// <summary>
    /// order Towers, Melee, Ranged
    /// </summary>
    [DataMember]
    readonly List<IMacroAction[]> possibleActions;

    ConcurrentQueue<int> AccumulatedFitnessesTowers;
    ConcurrentQueue<int> AccumulatedFitnessesMelee;
    ConcurrentQueue<int> AccumulatedFitnessesRanged;

    public CoevolutionAI(Individual tw, Individual melee, Individual ranged, List<IMacroAction[]> actions)
    {
        towers = tw;
        meleeUnits = melee;
        rangedUnits = ranged;

        possibleActions = actions;

        AccumulatedFitnessesTowers = new ConcurrentQueue<int>();
        AccumulatedFitnessesMelee = new ConcurrentQueue<int>();
        AccumulatedFitnessesRanged = new ConcurrentQueue<int>();
    }

    private CoevolutionAI DeepCopy()
    {
        Individual tw = new Individual(this.towers);
        Individual melee = new Individual(this.meleeUnits);
        Individual ranged = new Individual(this.rangedUnits);

        return new CoevolutionAI(tw, melee, ranged, possibleActions);
    }

    public override AIPlayer Clone()
    {
        return DeepCopy();
    }

    protected override IAction FindAction(Attacker attacker)
    {
        IAction resultAction = null;
        IMacroAction[] actions;
        Individual current;

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
            if (rule.AllTrue(attacker))
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

    public Tuple<int,int,int> GetFitnessMean()
    {
        int towers = MeanFitness(AccumulatedFitnessesTowers.ToArray());
        int melee = MeanFitness(AccumulatedFitnessesMelee.ToArray());
        int ranged = MeanFitness(AccumulatedFitnessesRanged.ToArray());

        return new Tuple<int, int, int>(towers, melee, ranged);
    }

    private int MeanFitness(int[] fitnessArray)
    {
        if (fitnessArray.Length == 1)
            return fitnessArray[0];

        Array.Sort(fitnessArray);
        return fitnessArray[Mathf.CeilToInt(fitnessArray.Length / 2)];
    }


    protected override void RunOver(GameStats stats)
    {
        GameStats towerStats = stats.FilterStatistics((x) => x.UnitType.IsSubclassOf(typeof(TowerBase)));
        GameStats meleeStats = stats.FilterStatistics((x) => x.UnitType.IsSubclassOf(typeof(HumanUnit)) && x.UnitType != typeof(Archers));
        GameStats rangedStats = stats.FilterStatistics((x) => x.UnitType == typeof(Archers));

        towers.SetFtiness(towerStats, role);
        meleeUnits.SetFtiness(meleeStats, role);
        rangedUnits.SetFtiness(rangedStats, role);

        AccumulatedFitnessesTowers.Enqueue(towers.Fitness);
        AccumulatedFitnessesMelee.Enqueue(meleeUnits.Fitness);
        AccumulatedFitnessesRanged.Enqueue(rangedUnits.Fitness);
            
    }

    protected override int PickToBuy()
    {
        return UnitFinder.PickOnBudget(OwnArmy.Money);
    }
}
