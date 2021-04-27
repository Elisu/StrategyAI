using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genetic;

public class CoevolutionAI : AIPlayer
{
    public int populationSize;
    public int individualLength;

    StrategyGroup towers;
    StrategyGroup meleeUnits;
    StrategyGroup rangedUnits;

    private void Awake()
    {
        //Towers
        TryAction[] actions = new TryAction[4] { MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions = new Condition[2] { Conditions.Damaged, Conditions.Free };
        towers = new StrategyGroup(populationSize, individualLength, actions, conditions);

        //Melee
        TryAction[] actions2 = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions2 = new Condition[2] { Conditions.Damaged, Conditions.Free };
        meleeUnits = new StrategyGroup(populationSize, individualLength, actions2, conditions2);

        //Ranged
        TryAction[] actions3 = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        Condition[] conditions3 = new Condition[2] { Conditions.Damaged, Conditions.Free };
        rangedUnits = new StrategyGroup(populationSize, individualLength, actions3, conditions3);


        runsPerGen = meleeUnits.GenerationRunCount;
    }


    protected override void OnStart()
    {
        towers.SelectIndividual();
        meleeUnits.SelectIndividual();
        rangedUnits.SelectIndividual();
    }

    protected override void FindAction(IAttack attacker)
    {
        StrategyGroup current;

        if (attacker is TowerBase)
            current = towers;
        else if (attacker is Troop<Archers>)
            current = rangedUnits;
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


    protected override void RunOver()
    {
        List<IRecruitable> dead = ownTroops.GetDead();

        List<Statistics> statsTowers = new List<Statistics>();
        List<Statistics> statsMelee = new List<Statistics>();
        List<Statistics> statsRanged = new List<Statistics>();

        for (int i = 0; i < dead.Count; i++)
        {
            IRecruitable corpse = dead[i];

            if (corpse is TowerBase)
                statsTowers.Add(corpse.GetStats());
            else if (corpse is Troop<Archers>)
                statsMelee.Add(corpse.GetStats());
            else
                statsRanged.Add(corpse.GetStats());
        }

        towers.SetFitness(statsTowers);
        meleeUnits.SetFitness(statsMelee);
        rangedUnits.SetFitness(statsRanged);

        if (towers.IsGenOver())
        {
            towers.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
            meleeUnits.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
            rangedUnits.GeneticOperations(RouletteWheel, UniformCrossover, ActionMutation);
        }
            
    }
}
