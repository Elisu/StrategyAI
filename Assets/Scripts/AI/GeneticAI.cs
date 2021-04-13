using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetic
{
    public delegate bool TryAction(IAttack recruit);
    public delegate bool Condition(IAttack recruit);

    public static Individual[] CreatePopulation(int popSize, int indSize, int possibleActionsCount, Condition[] conditions)
    {
        Individual[] population = new Individual[popSize];

        for (int i = 0; i < popSize; i++)
            population[i] = new Individual(indSize, possibleActionsCount, conditions);

        return population;
    }
    
    public class Individual : IEnumerable<Rule>
    {
        Rule[] rules;

        public Individual(int indLength, int possibleActionCount, Condition[] conditions)
        {
            rules = new Rule[indLength];

            for (int i = 0; i < indLength; i++)
            {
                rules[i] = new Rule(possibleActionCount, conditions);
            }
        }

        public IEnumerator<Rule> GetEnumerator()
        {
            foreach (Rule rule in rules)
            {
                yield return rule;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class Rule
    {
        Condition[] conditions { get; }
        public int ActionIndex { get; }

        public Rule(int possibleActionsCount, Condition[] possibleConditions)
        {
            conditions = new Condition[possibleConditions.Length];

            for (int i = 0; i < possibleConditions.Length; i++)
            {
                conditions[i] = possibleConditions[Random.Range(0, conditions.Length)];
            }

            ActionIndex = Random.Range(0, possibleActionsCount);
        }

        public bool AllTrue(IAttack recruit)
        {
            foreach (Condition cond in conditions)
                if (!cond(recruit))
                    return false;

            return true;
        }
    }

    public class Conditions
    {       
        public static bool Strongest(IAttack recruit)
        {
            return true;
        }

        public static bool Damaged(IAttack damaged)
        {
            if (damaged.ReceivedDamage > 0)
                return true;

            return false;
        }

        public static bool Free (IAttack attacker)
        {
            if (attacker.CurrentState == State.Free)
                return true;

            return false;
        }

    }

    public class StrategyGroup
    {
        public int individualLength { get; protected set; }
        public int populationSize { get; protected set; }

        public TryAction[] possibleActions;
        public Condition[] usedConditions;
        public Individual[] population;

        public Individual individual;

        public StrategyGroup(int popSize, int indLength, TryAction[] actions, Condition[] conditions)
        {
            possibleActions = actions;
            usedConditions = conditions;
            population = CreatePopulation(popSize, indLength, actions.Length, conditions);
            individual = population[0];
        }

        public void GeneticOperations()
        {
           // Individual[] selected = selection;
           // mutate(selection)
           // 
        }
    }


}
