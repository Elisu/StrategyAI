using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genetic
{
    public delegate T[] Selection<T>(T[] pop) where T : Individual<T>;
    public delegate T MutateFunc<T>(T ind) where T : Individual<T>;
    public delegate Tuple<T, T> CrossFunc<T>(T a, T b) where T : Individual<T>;

    public interface Individual<T>
    {
        public T GetClone(); 

        int Fitness { get; }
    }

    public class EvolutionFunctions
    {
        public static T[] RouletteWheelSelection<T>(T[] pop) where T : Individual<T>
        {
            T[] selected = new T[pop.Length];
            double fitnessSum = 0;
            double[] fitnesses = new double[pop.Length];

            for (int i = 0; i < pop.Length; i++)
            {
                fitnessSum += pop[i].Fitness;
            }

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
                        selected[i] = pop[j].GetClone();
                        break;
                    }
                }

            }

            return selected;
        }

        public static T[] Crossover<T>(T[] pop, CrossFunc<T> cross) where T : Individual<T>
        {
            T[] crossed = new T[pop.Length];

            for (int i = 1; i < pop.Length; i += 2)
            {
                if (UnityEngine.Random.value < 0.4)
                {
                    Tuple<T, T> offs = cross(pop[i - 1], pop[i]);
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

        public static T[] Mutation<T>(T[] pop, MutateFunc<T> mutate) where T : Individual<T>
        {
            List<T> mutated = new List<T>();

            foreach (T ind in pop)
            {
                if (UnityEngine.Random.value < 0.25)
                    mutated.Add(mutate(ind));
                else
                    mutated.Add(ind.GetClone());
            }

            return mutated.ToArray();
        }

        public static int ComputeFitness(GameStats stats, Role role)
        {
            int fitnessResult = 0;

            IList<Statistics> ownStats = stats.GetMyStats(role);

            if (stats.Winner == role)
                fitnessResult += 15000;

            int statResults = 0;
            foreach (Statistics stat in ownStats)
                statResults += stat.dealtDamage + stat.destroyedBuildings * 100 + stat.killedEnemies * 1000;

            if (ownStats.Count > 0)
                statResults /= ownStats.Count;

            return fitnessResult + statResults;
        }
    }
}

