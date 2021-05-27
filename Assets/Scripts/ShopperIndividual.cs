using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class ShopperIndividual : Genetic.Individual<ShopperIndividual>
{
    public int Fitness { get; private set; } = 0;

    public int Length => shoppingOrder.Length;

    [DataMember]
    readonly int[] shoppingOrder;

    readonly List<int> fitnesses = new List<int>();

    int next;

    public ShopperIndividual()
    {
        next = -1;
        shoppingOrder = new int[UnitFinder.unitStats.Count];

        for (int i = 0; i < shoppingOrder.Length; i++)
        {
            shoppingOrder[i] = i;
        }

        Shuffle();
    }

    public int this[int index]
    {
        get => shoppingOrder[index];
    }

    public ShopperIndividual(int[] values)
    {
        next = -1;
        shoppingOrder = values;
    }

    private void Shuffle()
    {
        for (int i = 0; i < shoppingOrder.Length; i++)
        {
            int tmp = shoppingOrder[i];
            int r = Random.Range(i, shoppingOrder.Length);
            shoppingOrder[i] = shoppingOrder[r];
            shoppingOrder[r] = tmp;
        }
    }

    public int GetNext(int budget)
    {
        for (int i = 0; i < shoppingOrder.Length; i++)
        {
            next = (next + 1) % shoppingOrder.Length;

            if (UnitFinder.unitStats[next].Price <= budget)
                return shoppingOrder[next];
        }

        return 0;
    }

    public ShopperIndividual GetClone()
    {
        return new ShopperIndividual(shoppingOrder);
    }

    public void AddFitness(int fitness)
    {
        fitnesses.Add(fitness);
    }

    public void Evaluate()
    {
        if (fitnesses.Count == 0)
            return;

        fitnesses.Sort();
        Fitness = fitnesses[fitnesses.Count / 2];
    }
}
