using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesAI : AIBase
{
    delegate bool TryAction(IAttack recruit);
    delegate bool Condition(IAttack recruit);

    TryAction[] possibleActions;
    Individual[] population;

    class Individual
    {
        Rule[] rules;

        public Individual()
        {
            for (int i = 0; i < 5; i++)
            {
                rules[i] = new Rule();
            }
        }
    }

    class Rule
    {
        Condition[] conditions { get; }
        int actionIndex { get; }

        public Rule()
        {
            for (int i = 0; i < 5; i++)
            {
                conditions[i] = Conditions.availableConditions[Random.Range(0, Conditions.availableCondCount)];
            }

            actionIndex = Random.Range(0, 1);
        }
    }

    class Conditions
    {
        public const int availableCondCount = 1;
        public static Condition[] availableConditions = new Condition[availableCondCount] { Strongest };


        static bool Strongest(IAttack recruit)
        {
            return true;
        }

    }

    void Awake()
    {
        possibleActions = new TryAction[2] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth };  
    }

    protected override void FindAction()
    {
        foreach (IAttack recruit in troops)
        {
            int[] votes = new int[possibleActions.Length];

            //To DO

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
