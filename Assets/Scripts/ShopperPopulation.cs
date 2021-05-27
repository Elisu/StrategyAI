using Genetic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopperPopulation
{
    ShopperIndividual[] population;

    public ShopperPopulation(int length)
    {
        population = new ShopperIndividual[length];

        for (int i = 0; i < length; i++)
            population[i] = new ShopperIndividual();
    }

    public ShopperIndividual this[int index]
    {
        get => population[index];
    }

    public void Evolve()
    {
        for (int i = 0; i < population.Length; i++)
            population[i].Evaluate();

        ShopperIndividual[] selected = EvolutionFunctions.RouletteWheelSelection(population);

        selected = EvolutionFunctions.Crossover(selected, OnePointCross);
        selected = EvolutionFunctions.Mutation(selected, ShuffleMutation);
        population = selected;
    }

    private Tuple<ShopperIndividual, ShopperIndividual> OnePointCross(ShopperIndividual a, ShopperIndividual b)
    {
        return new Tuple<ShopperIndividual, ShopperIndividual>(a, b);
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
}
