using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Genetic
{
    //public delegate bool TryAction(Attacker recruit, out IAction action);

    public class Strategy
    {
        public int PopulationSize => population.Length;

        public int GenerationRunCount => population.Length * 1;

        public ReadOnlyCollection<StrategyIndividual> Population => Array.AsReadOnly<StrategyIndividual>(population);

        public StrategyIndividual Champion { get; private set; }
        private StrategyIndividual[] population;

        float crossProb;
        float mutationProb;
        float actionMutationProb;

        public Strategy(int popSize, int indLength, int actionPossibilitiesCount, ICondition[] conditions, float crossProbability, 
                        float mutationProbability, float actionMutationProbability)
        { 
            population = StrategyIndividual.CreatePopulation(popSize, indLength, actionPossibilitiesCount, conditions);
            Champion = population[0];
            crossProb = crossProbability;
            mutationProb = mutationProbability;
            actionMutationProb = actionMutationProbability;
        }

        public StrategyIndividual this[int index]
        {
            get => population[index];
        }

        public void Evolve(bool elitism = false)
        {
            StrategyIndividual best = population[0];

            foreach (StrategyIndividual ind in population)
            {
                ind.Evaluate();

                if (ind.Fitness > best.Fitness)
                    best = ind;
            }

            Champion = best;

            StrategyIndividual[] selected = EvolutionFunctions.RouletteWheelSelection(population);

            if (selected == null)
                return;

            selected = EvolutionFunctions.Crossover(selected, UniformCrossover, crossProb);
            selected = EvolutionFunctions.Mutation(selected, ActionMutation, mutationProb);
            population = selected;

            if (elitism)
                population[0] = new StrategyIndividual(Champion);
        }      


        private StrategyIndividual ActionMutation(StrategyIndividual ind)
        {
            StrategyIndividual mutated = new StrategyIndividual(ind);

            foreach (Rule rule in mutated)
            {
                if (UnityEngine.Random.value < actionMutationProb)
                    rule.ActionIndex = UnityEngine.Random.Range(0, rule.ActionCount);
            }

            return mutated;
        }

        private Tuple<StrategyIndividual, StrategyIndividual> UniformCrossover(StrategyIndividual a, StrategyIndividual b)
        {
            List<Rule> first = new List<Rule>();
            List<Rule> second = new List<Rule>();

            for (int i = 0; i < Mathf.Min(a.Length, b.Length); i++)
            {
                if (UnityEngine.Random.value > 0.5)
                {
                    first.Add(new Rule(a[i]));
                    second.Add(new Rule(b[i]));
                }
                else
                {
                    first.Add(new Rule(b[i]));
                    second.Add(new Rule(a[i]));
                }
            }

            int end;
            StrategyIndividual rest;

            if (a.Length < b.Length)
            {
                end = a.Length;
                rest = b;
            }
            else
            {
                end = b.Length;
                rest = a;
            }

            for (int i = end; i < rest.Length; i++)
            {
                if (UnityEngine.Random.value > 0.5)
                    first.Add(new Rule(rest[i]));
                else
                    second.Add(new Rule(rest[i]));
            }

            return new Tuple<StrategyIndividual, StrategyIndividual>(new StrategyIndividual(first), new StrategyIndividual(second));
        }

    }
}

