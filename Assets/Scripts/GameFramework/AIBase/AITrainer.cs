using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;

public abstract class AITrainer : MonoBehaviour
{
    protected List<AIPlayer> population = new List<AIPlayer>();

    public ReadOnlyCollection<AIPlayer> Population => population.AsReadOnly();

    public abstract Type AIPlayerType { get; }

    protected internal virtual void OnStart()
    {
        InitializeVariables();
        BeforePopCreation();
        population = CreatPopulation();
        AfterPopCreation();
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
    protected virtual void AfterPopCreation()
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

    protected internal virtual void SaveChampion()
    {
        AIPlayer player = GetChampion();

        if (player == null)
            return;

        using (var stream = new FileStream(string.Format("{0}", player.ToString()), FileMode.Create))
        {
            DataContractSerializer serializer = new DataContractSerializer(player.GetType());
            serializer.WriteObject(stream, player);
        }
    }

    public virtual AIPlayer LoadChampion()
    {
        using (var stream = new FileStream(string.Format("{0}", AIPlayerType.Name), FileMode.Open))
        {
            DataContractSerializer serializer = new DataContractSerializer(AIPlayerType);
            return (AIPlayer)serializer.ReadObject(stream);
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

    /// <summary>
    /// Called at the end of the training to retreive the player to save
    /// </summary>
    /// <returns>Player to be saved at the end of the training - null means nothing saved</returns>
    public abstract AIPlayer GetChampion();
}
