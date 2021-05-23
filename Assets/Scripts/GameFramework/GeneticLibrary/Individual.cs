using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Genetic
{
    public delegate int FitnessCalculation(GameStats stats, Role role);

    [DataContract]
    public class Individual : IEnumerable<Rule>
    {
        public int Fitness { get; set; }
        public int Length => rules.Length;

        [DataMember]
        public int PossibleActions { get; private set; }

        [DataMember]
        Rule[] rules;

        FitnessCalculation customFitness;

        public static Individual[] CreatePopulation(int popSize, int indSize, int possibleActionsCount, ICondition[] conditions)
        {
            Individual[] population = new Individual[popSize];

            for (int i = 0; i < popSize; i++)
                population[i] = new Individual(indSize, possibleActionsCount, conditions);

            return population;
        }

        public Individual(int indLength, int possibleActionCount, ICondition[] conditions, FitnessCalculation fitnessMath = null)
        {
            customFitness = fitnessMath;
            rules = new Rule[indLength];
            PossibleActions = possibleActionCount;

            for (int i = 0; i < indLength; i++)
            {
                rules[i] = new Rule(possibleActionCount, conditions);
            }
        }

        /// <summary>
        /// Copy contructor for individual
        /// </summary>
        /// <param name="ind"></param>
        public Individual(Individual ind)
        {
            rules = new Rule[ind.Length];
            PossibleActions = ind.PossibleActions;
            customFitness = ind.customFitness;

            for (int i = 0; i < ind.Length; i++)
                rules[i] = new Rule(ind.rules[i]);
        }

        public Individual(List<Rule> rules)
        {
            this.rules = rules.ToArray();
            PossibleActions = rules[0].ActionCount;
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

        public void SetFtiness(GameStats stats, Role role)
        {
            if (customFitness != null)
                Fitness = customFitness.Invoke(stats, role);
            else
                Fitness = GetFitness(stats, role);

            //Debug.Log(string.Format("Fitness: {0}", Fitness));
        }

        private int GetFitness(GameStats stats, Role role)
        {
            int fitnessResult = 0;

            List<Statistics> ownStats = stats.GetMyStats(role);

            if (stats.Winner == role)
                fitnessResult += 15000;

            foreach (Statistics stat in ownStats)
                fitnessResult += stat.dealtDamage + stat.destroyedBuildings * 100 + stat.killedEnemies * 1000;

            return fitnessResult;
        }
    }
}

