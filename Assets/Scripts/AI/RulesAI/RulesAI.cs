using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class RulesAI : AIPlayer
{
    Individual individual;
    TryAction[] possibleActions;

    public RulesAI(Individual ind, TryAction[] actions)
    {
        possibleActions = actions;
        individual = ind;
    }

    public override AIPlayer Clone()
    {
        return new RulesAI(new Individual(individual), possibleActions);
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

        possibleActions[indexOfBest].Invoke(attacker, out resultAction);

        return resultAction;
    }

    protected override void RunOver()
    {
        List<IRecruitable> dead = OwnArmy.GetDead();

        List<Statistics> stats = new List<Statistics>();
        for (int i = 0; i < dead.Count; i++)
            stats[i] = dead[i].GetStats();

       // individual.SetFitness(stats);

    }





}
