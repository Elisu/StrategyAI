using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetic
{
    public delegate Individual[] Selection(Individual[] pop);
    public delegate Individual MutateFunc(Individual ind);
    public delegate Tuple<Individual, Individual> CrossFunc(Individual a, Individual b);

    //public delegate bool TryAction(Attacker recruit, out IAction action);

    public class Strategy
    {
        public int IndividualLength { get; protected set; }
        public int PopulationSize => population.Length;

        public int GenerationRunCount => population.Length * 1;

        public IMacroAction[] possibleActions;
        public ICondition[] usedConditions;
        public Individual[] population;

        public Strategy(int popSize, int indLength, IMacroAction[] actions, ICondition[] conditions)
        {
            possibleActions = actions;
            usedConditions = conditions;
            population = Individual.CreatePopulation(popSize, indLength, actions.Length, conditions);
        }

        public Individual this[int index]
        {
            get => population[index];
        }

        public void GeneticOperations(Selection selection, CrossFunc cross, MutateFunc mutate)
        {
            //Debug.LogWarning("GENETIC");
            Individual[] selected = selection(population);

            if (selected == null)
                return;

            selected = Crossover(selected, cross);
            selected = Mutation(selected, mutate);
            population = selected;
        }

        private Individual[] Crossover(Individual[] pop, CrossFunc cross)
        {
            Individual[] crossed = new Individual[pop.Length];

            for (int i = 1; i < pop.Length; i += 2)
            {
                if (UnityEngine.Random.value < 0.4)
                {
                    Tuple<Individual, Individual> offs = cross(pop[i - 1], pop[i]);
                    crossed[i - 1] = offs.Item1;
                    crossed[i] = offs.Item2;
                }
                else
                {
                    crossed[i - 1] = pop[i - 1];
                    crossed[i] = pop[i];
                }

            }

            //If length not even add last individual
            if (pop.Length % 2 == 1)
                crossed[pop.Length - 1] = pop[pop.Length - 1];

            return crossed;
        }

        private Individual[] Mutation(Individual[] pop, MutateFunc mutate)
        {
            List<Individual> mutated = new List<Individual>();

            foreach (Individual ind in pop)
            {
                if (UnityEngine.Random.value < 0.25)
                    mutated.Add(mutate(ind));
                else
                    mutated.Add(new Individual(ind));
            }

            return mutated.ToArray();
        }

    }
}

