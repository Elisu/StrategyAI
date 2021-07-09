using Genetic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopperPopulation
{
    ShopperIndividual[] population;

    float crossProb;
    float mutationProb;

    public ShopperPopulation(int length, float crossProbability, float mutationProbability)
    {
        population = new ShopperIndividual[length];
        crossProb = crossProbability;
        mutationProb = mutationProbability;

        for (int i = 0; i < length; i++)
            population[i] = new ShopperIndividual();
    }

    public ShopperIndividual this[int index]
    {
        get => population[index];
    }

    public void Evolve(bool elitism = false)
    {
        ShopperIndividual best = population[0];

        for (int i = 0; i < population.Length; i++)
        {
            population[i].Evaluate();

            if (population[i].Fitness > best.Fitness)
                best = population[i];
        }           

        ShopperIndividual[] selected = EvolutionFunctions.RouletteWheelSelection(population);

        selected = EvolutionFunctions.Crossover(selected, OnePointCross, crossProb);
        selected = EvolutionFunctions.Mutation(selected, ValueMutation, mutationProb);
        population = selected;

        if (elitism)
            population[0] = best;
    }

    private Tuple<ShopperIndividual, ShopperIndividual> OnePointCross(ShopperIndividual a, ShopperIndividual b)
    {
        int crossPoint = UnityEngine.Random.Range(0, a.Length);

        int[] first = new int[a.Length];
        int[] second = new int[b.Length];

        for (int i = 0; i < crossPoint; i++)
        {
            first[i] = a[i];
            second[i] = b[i];
        }

        for (int i = crossPoint; i < a.Length; i++)
        {
            first[i] = b[i];
            second[i] = a[i];
        }

        return new Tuple<ShopperIndividual, ShopperIndividual>(new ShopperIndividual(first), new ShopperIndividual(second));
    }

    private ShopperIndividual ShuffleMutation(ShopperIndividual ind)
    {
        int[] shuffled = new int[ind.Length];

        for (int i = 0; i < ind.Length; i++)
            shuffled[i] = ind[i];

        for (int i = 0; i < ind.Length; i++)
        {
            int temp = shuffled[i];
            int r = UnityEngine.Random.Range(i, ind.Length);
            shuffled[i] = shuffled[r];
            shuffled[r] = temp;
        }

        return new ShopperIndividual(shuffled);
    }

    private ShopperIndividual ValueMutation(ShopperIndividual ind)
    {
        int[] mutated = new int[ind.Length];

        for (int i = 0; i < mutated.Length; i++)
            mutated[i] = ind[i];

        mutated[UnityEngine.Random.Range(0, mutated.Length)] = UnityEngine.Random.Range(0, UnitFinder.UnitStats.Count);

        return new ShopperIndividual(mutated);
    }
}
