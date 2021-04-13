using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class RulesAI : AIBase
{
    public int populationSize;
    public int individualLength;

    StrategyGroup all;

    private void Awake()
    {
        TryAction[] actions = new TryAction[2] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth };
        Condition[] conditions = new Condition[2] { Conditions.Damaged, Conditions.Free };
        all = new StrategyGroup(populationSize, individualLength, actions, conditions);
    }

    private void OnEnable()
    {
        MasterScript.GameOver += RunOver;
    }

    private void OnDisable()
    {
        MasterScript.GameOver -= RunOver;
    }

    protected override void OnStart()
    {

    }

    protected override void FindAction(IAttack attacker)
    {
        int[] votes = new int[all.possibleActions.Length];

        foreach (Rule rule in all.individual)
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

        all.possibleActions[indexOfBest].Invoke(attacker);
        
    }

    private void RunOver()
    {
        
    }





}
