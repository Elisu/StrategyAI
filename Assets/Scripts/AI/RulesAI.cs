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
        TryAction[] actions = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions = new Condition[2] { Conditions.Damaged, Conditions.Free };
        all = new StrategyGroup(populationSize, individualLength, actions, conditions);
        runsPerGen = all.GenerationRunCount;
    }
    

    protected override void OnStart()
    {
        all.SelectIndividual();
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

    protected override void RunOver()
    {
        List<IRecruitable> dead = ownTroops.GetDead();

        List<Statistics> stats = new List<Statistics>();
        for (int i = 0; i < dead.Count; i++)
            stats[i] = dead[i].GetStats();

        all.SetFitness(stats);

        if (all.IsGenOver())  
            all.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
    }





}
