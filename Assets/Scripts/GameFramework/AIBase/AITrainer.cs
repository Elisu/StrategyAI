using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;

public abstract class AITrainer : AIController
{
    protected List<AIPlayer> population = new List<AIPlayer>();

    public override ReadOnlyCollection<AIPlayer> Population => population.AsReadOnly();

    protected internal override void OnStart()
    {
        InitializeVariables();
        CustomStart();
        population = CreatePopulation();
        AfterPopCreation();
        BeforeEachGeneration();
    }

    private void InitializeVariables()
    {
        Type t = this.GetType();
    }

    /// <summary>
    /// Method called once after the instantiation of the class, right after CreatePopulation -
    /// override if needed
    /// </summary>
    protected virtual void AfterPopCreation()
    {
        return;
    }

    public void Save()
    {
        try
        {
            string directoryPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Trained", this.GetType().Name);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            SaveChampion(Path.Combine(directoryPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture)) + ".xml");
        }
        catch (Exception _e)
        {
            Debug.LogError("Saving failed");
        }

        Debug.Log(string.Format("{0} champion saved", this.AIPlayerType.Name));
    }

    protected internal virtual void SaveChampion(string file)
    {
        AIPlayer player = GetChampion();

        if (player == null)
            return;

        using (var stream = new FileStream(file, FileMode.Create))
        {
            DataContractSerializer serializer = new DataContractSerializer(player.GetType());
            serializer.WriteObject(stream, player);
        }
    }

    public override IPlayer Load(string championFile)
    {
        if (string.IsNullOrEmpty(championFile))
            return GetPlayer();

        string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Trained", this.GetType().Name, championFile);
        return LoadChampion(path);
    }

    public virtual IPlayer LoadChampion(string file)
    {
        using (var stream = new FileStream(file, FileMode.Open))
        {
            DataContractSerializer serializer = new DataContractSerializer(AIPlayerType);
            return (IPlayer)serializer.ReadObject(stream);
        }
    }

    public override IPlayer GetPlayer()
    {
        var pop = CreatePopulation();
        return pop[UnityEngine.Random.Range(0, pop.Count)];
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract List<AIPlayer> CreatePopulation();

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
