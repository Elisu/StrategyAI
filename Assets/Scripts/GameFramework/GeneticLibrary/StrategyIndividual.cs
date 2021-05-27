using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Genetic
{
    public delegate int FitnessCalculation(GameStats[] stats, Role role);

    [DataContract]
    public class StrategyIndividual : Individual<StrategyIndividual>, IEnumerable<Rule>
    {
        public int Fitness { get; private set; } = 0;

        public int Length => rules.Length;

        [DataMember]
        public readonly int PossibleActions;

        [DataMember]
        readonly Rule[] rules;

        readonly List<int> fitnesses = new List<int>();

        public static StrategyIndividual[] CreatePopulation(int popSize, int indSize, int possibleActionsCount, ICondition[] conditions)
        {
            StrategyIndividual[] population = new StrategyIndividual[popSize];

            for (int i = 0; i < popSize; i++)
                population[i] = new StrategyIndividual(indSize, possibleActionsCount, conditions);

            return population;
        }

        public StrategyIndividual(int indLength, int possibleActionCount, ICondition[] conditions)
        {
            rules = new Rule[indLength];
            PossibleActions = possibleActionCount;

            for (int i = 0; i < indLength; i++)
            {
                rules[i] = new Rule(possibleActionCount, conditions);
            }
        }

        /// <summary>
        /// Copy contructor for individual - you can decide if you want to keep colected fitness
        /// </summary>
        /// <param name="ind"></param>
        public StrategyIndividual(StrategyIndividual ind)
        {
            rules = new Rule[ind.Length];
            PossibleActions = ind.PossibleActions;

            for (int i = 0; i < ind.Length; i++)
                rules[i] = new Rule(ind.rules[i]);
        }

        public StrategyIndividual(List<Rule> rules)
        {
            this.rules = rules.ToArray();
            PossibleActions = rules[0].ActionCount;
        }

        public StrategyIndividual GetClone()
        {
            return new StrategyIndividual(this);
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

        public Rule this[int index] => rules[index];

        public void AddFitness(int fitness)
        {
            fitnesses.Add(fitness);
        }

        public void  Evaluate()
        {
            if (fitnesses.Count == 0)
                return;

            fitnesses.Sort();
            Fitness = fitnesses[fitnesses.Count / 2];
        }

    }
}

