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
        

    /// <summary>
    /// This class implements different genetic operators that 
    /// can be used in the implementation of a genetic algortihm
    /// </summary>
    public class EvolutionFunctions
    {
        public static  T[] TournamentSelection<T>(T[] pop) where T : Individual<T>
        {
            T[] selected = new T[pop.Length];

            for (int i = 0; i < pop.Length; i++)
            {
                int opponent = UnityEngine.Random.Range(0, pop.Length);

                if (pop[i].Fitness > pop[opponent].Fitness)
                    selected[i] = pop[i].GetClone();
                else
                    selected[i] = pop[opponent].GetClone();
            }

            return selected;
        }

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

        public static T[] Crossover<T>(T[] pop, CrossFunc<T> cross, float prob) where T : Individual<T>
        {
            T[] crossed = new T[pop.Length];

            for (int i = 1; i < pop.Length; i += 2)
            {
                if (UnityEngine.Random.value < prob)
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

        public static T[] Mutation<T>(T[] pop, MutateFunc<T> mutate, float prob) where T : Individual<T>
        {
            List<T> mutated = new List<T>();

            foreach (T ind in pop)
            {
                if (UnityEngine.Random.value < prob)
                    mutated.Add(mutate(ind));
                else
                    mutated.Add(ind.GetClone());
            }

            return mutated.ToArray();
        }

        public static Tuple<int, int, int, int, Role> ComputeFitness(GameStats stats, Role role)
        {
            int fitnessResult = 0;

            IList<Statistics> ownStats = stats.GetMyStats(role);
            IList<Statistics> enemyStats = stats.GetEnemyStats(role);

            if (stats.Winner == role)
                fitnessResult += 400000;

            int ownDamage = 0;
            int enemyDamage = 0;
            int enemiesKilled = 0;

            foreach (Statistics stat in ownStats)
            {
                ownDamage += stat.dealtDamage;

                if (stat.UnitType.IsSubclassOf(typeof(HumanUnit)))
                    enemyDamage += stat.receivedDamage;

                enemiesKilled += stat.killedEnemies;
            }

            //foreach (Statistics stat in enemyStats)
            //    if (stat.UnitType.IsSubclassOf(typeof(HumanUnit)))
            //        enemyDamage += stat.dealtDamage;

            int damageDiff = ownDamage - enemyDamage;

            //if (stats.Winner == Role.Neutral)
            //    damageDiff /= 10;

            return new Tuple<int,int,int, int, Role>(fitnessResult + Math.Max(0, damageDiff) + enemiesKilled * 10000, damageDiff, enemiesKilled, stats.LeftFrames, stats.Winner);
        }
    }
}

