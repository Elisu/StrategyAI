using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITrainingHandler : MonoBehaviour
{ 
    protected AIPlayer AI;

    protected List<AIPlayer> population;

    public int PopulationCount => population.Count;

    protected void Start()
    {
        population = CreatPopulation();
    }

    public List<AIPlayer> GetPopulation()
    {
        return population;
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract List<AIPlayer> CreatPopulation();

    public abstract void GenerationDone();

    public abstract AIPlayer GetRepresentative();
}
