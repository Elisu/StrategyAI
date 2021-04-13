using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class CoevolutionAI : AIBase
{
    StrategyGroup towers;
    StrategyGroup meleeUnits;
    StrategyGroup rangedUnits;

    private void Awake()
    {
        
    }

    protected override void FindAction(IAttack attacker)
    {
        StrategyGroup current;

        if (attacker is Tower)
            current = towers;
        else
            current = meleeUnits;

        int[] votes = new int[current.possibleActions.Length];

        foreach (Rule rule in current.individual)
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

        current.possibleActions[indexOfBest].Invoke(attacker);
    }
}
