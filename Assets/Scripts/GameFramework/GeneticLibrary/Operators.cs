using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetic
{
    public class Operators
    {
        public static Individual[] RouletteWheel(Individual[] pop)
        {
            Individual[] selected = new Individual[pop.Length];
            double fitnessSum = 0;
            double[] fitnesses = new double[pop.Length];

            for (int i = 0; i < pop.Length; i++)
                fitnessSum += pop[i].Fitness;

            if (fitnessSum == 0)
                return null;

            for (int i = 0; i < pop.Length; i++)
                fitnesses[i] = pop[i].Fitness / fitnessSum;

            for (int i = 0; i < pop.Length; i++)
            {
                double ball = UnityEngine.Random.value;
                double sum = 0;

                for (int j = 0; j < fitnesses.Length; j++)
                {
                    sum += fitnesses[j];

                    if (sum > ball)
                    {
                        selected[i] = new Individual(pop[j]);
                        break;
                    }
                }

            }

            return selected;
        }

        public static Individual ActionMutation(Individual ind)
        {
            Individual mutated = new Individual(ind);

            foreach (Rule rule in mutated)
            {
                if (UnityEngine.Random.value < 0.5)
                    rule.ActionIndex = UnityEngine.Random.Range(0, rule.ActionCount);
            }

            return mutated;
        }

        public static Tuple<Individual, Individual> UniformCrossover(Individual a, Individual b)
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
            Individual rest;

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

            return new Tuple<Individual, Individual>(new Individual(first), new Individual(second));
        }
    }
}

