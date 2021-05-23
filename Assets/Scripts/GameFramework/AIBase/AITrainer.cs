using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;

public abstract class AITrainer : MonoBehaviour
{
    public List<AIPlayer> Population { get; private set; }

    public int PopulationCount => Population.Count;

    public abstract Type AIPlayerType { get; }

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

    protected internal virtual void SaveGiven()
    {
        AIPlayer player = ToSave();

        if (player == null)
            return;

        using (var stream = new FileStream("neco.xml", FileMode.Create))
        {
            DataContractSerializer serializer = new DataContractSerializer(player.GetType());
            serializer.WriteObject(stream, player);
        }
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

    public abstract AIPlayer ToSave();
}
