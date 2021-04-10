using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class RulesAI : AIBase
{
    public int individualLength;
    public int populationSize;

    TryAction[] possibleActions;
    Condition[] usedConditions;
    Individual[] population;

    Individual individual;

    

    void Start()
    {
        possibleActions = new TryAction[2] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth };
        usedConditions = new Condition[1] { Conditions.Damaged };
        population = CreatePopulation(populationSize, individualLength, possibleActions.Length, usedConditions);
    }

    protected override void FindAction()
    {
        foreach (IAttack recruit in ownTroops)
        {
            int[] votes = new int[possibleActions.Length];

            foreach (Rule rule in individual)
            {
                if (rule.AllTrue(recruit))
                    votes[rule.ActionIndex]++;
            }

            int indexOfBest = 0;

            for (int i = 0; i < votes.Length; i++)
            {
                if (votes[indexOfBest] < votes[i])
                    indexOfBest = i;
            }

            possibleActions[indexOfBest].Invoke(recruit);
        }

    }





}
