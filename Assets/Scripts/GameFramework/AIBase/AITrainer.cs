using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITrainer : MonoBehaviour
{
    public List<AIPlayer> Population { get; private set; }

    public int PopulationCount => Population.Count;

    protected internal virtual void OnStart()
    {
        InitializeVariables();
        BeforePopCreation();
        Population = CreatPopulation();
        AfterStart();
        BeforeEachGeneration();
    }

    private void InitializeVariables()
    {
        Type t = this.GetType();
    }

    protected virtual void BeforePopCreation()
    {
        return;
    }

    /// <summary>
    /// Method called once after the instantiation of the class, right after CreatePopulation -
    /// override if needed
    /// </summary>
    protected virtual void AfterStart()
    {
        return;
    }

    /// <summary>
    /// Method called before the start of each generation - override if needed
    /// </summary>
    protected internal virtual void BeforeEachGeneration()
    {
        return;
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract List<AIPlayer> CreatPopulation();

    /// <summary>
    /// Method called after generation is finished - good for your the generation evaluation code
    /// </summary>
    public abstract void GenerationDone();

    public abstract AIPlayer GetRepresentative();
}
