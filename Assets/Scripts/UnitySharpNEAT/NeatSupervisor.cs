/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Xml;
using System.IO;
using System.Linq;
using SharpNeat.Core;
using Genetic;
using System.Globalization;

namespace UnitySharpNEAT
{
    /// <summary>
    /// This class acts as the entry point for the NEAT evolution.
    /// It manages the UnitController's being evolved and handles the creation of the NeatEvolutionAlgorithm.
    /// </summary>
    public class NeatSupervisor : AITrainer
    {
        #region FIELDS
        [Header("Experiment Settings")]

        [SerializeField]
        private string _experimentConfigFileName = "experiment.config";

        public int PopulationSize;

        readonly List<int> fitnessesRecord = new List<int>();
        readonly List<Tuple<int, int, int, int, Role>> accumulatedStats = new List<Tuple<int, int, int, int, Role>>();


        private readonly ICondition[] inputConditions = new ICondition[12] { new Conditions.Damaged(),
                                                        new Conditions.Free(),
                                                        new Conditions.StrongerThanClosest(),
                                                        new Conditions.ClosestIsTroopBase(),
                                                        new Conditions.ClosestIsBuilding(),
                                                        new Conditions.ClosestIsTower(),
                                                        new Conditions.IsDefender(),
                                                        new Conditions.HealthierThanClosest(),
                                                        new Conditions.IsAlone(),
                                                        new Conditions.IsWinning(),
                                                        new Conditions.IsInsideCastle(),
                                                        new Conditions.IsInTowerRange() };

        private readonly IMacroAction[] outputActions = new IMacroAction[7] { new SerializableMacroActions.AttackClosest(),
                                                         new SerializableMacroActions.AttackWithLowestHealth(),
                                                         new SerializableMacroActions.AttackWithLowestDamage(),
                                                         new SerializableMacroActions.AttackWeakestAgainstMe(),
                                                         new SerializableMacroActions.AttackInRange(),
                                                         new SerializableMacroActions.DoNothing(),
                                                         new SerializableMacroActions.MoveToSafety() };






        private int _networkInputCount => inputConditions.Length;
        private int _networkOutputCount => outputActions.Length;

        // Object pooling and Unit management
        private Dictionary<IBlackBox, INeatPlayer> _blackBoxMap = new Dictionary<IBlackBox, INeatPlayer>();
        #endregion

        public NeatEvolutionAlgorithm<NeatGenome, IBlackBox> EvolutionAlgorithm { get; private set; }

        public Experiment Experiment { get; private set; }

        public override Type AIPlayerType => typeof(NeatAI);



        private void LoadExperiment()
        {
            XmlDocument xmlConfig = new XmlDocument();
            TextAsset textAsset = (TextAsset)Resources.Load(_experimentConfigFileName);

            if (textAsset == null)
            {
                Debug.LogError("The experiment config file named '" + _experimentConfigFileName + ".xml' could not be found in any Resources folder!");
                return;
            }

            xmlConfig.LoadXml(textAsset.text);

            Experiment = new Experiment();
            Experiment.Initialize(xmlConfig.DocumentElement, this, _networkInputCount, _networkOutputCount);

            ExperimentIO.DebugPrintSavePaths(Experiment);
        }
        #region NEAT LIFECYCLE
        /// <summary>
        /// Starts the NEAT algorithm.
        /// </summary>
        public void StartEvolution()
        {
            LoadExperiment();
            EvolutionAlgorithm = Experiment.CreateEvolutionAlgorithm(ExperimentIO.GetSaveFilePath(Experiment.Name, ExperimentFileType.Population));
        }

        protected override List<AIPlayer> CreatePopulation()
        {
            StartEvolution();
            return MakePopulation();
            
        }

        private List<AIPlayer> MakePopulation()
        {
            List<INeatPlayer> pop = new List<INeatPlayer>();

            List<IBlackBox> brains = EvolutionAlgorithm.GetIBlackBoxes(PopulationSize);

            for (int i = 0; i < PopulationSize; i++)
                pop.Add(new NeatAI(outputActions, inputConditions, brains[i]));

            return new List<AIPlayer>(pop);
        }

        protected override void BeforeEachGeneration() => StartCoroutine(EvolutionAlgorithm.PerformOneGeneration());

        public override void GenerationDone()
        {
            EvolutionAlgorithm.Evaluate(population);

            foreach(var ind in population)
                accumulatedStats.AddRange(((NeatAI)ind).AccumulattedGetStats());

            accumulatedStats.Add(new Tuple<int, int, int, int, Role>(-1, -1, -1, -1, Role.Neutral));

            population = MakePopulation();
        }


        protected override void SaveChampion(string file)
        {
            ExperimentIO.WriteChampion(Experiment, EvolutionAlgorithm.CurrentChampGenome, file);
        }

        public override IPlayer LoadChampion(string file)
        {
            LoadExperiment();
            NeatGenome championBrain = Experiment.LoadChampion(file);
            return new NeatAI(outputActions, inputConditions, Experiment.CreateGenomeDecoder().Decode(championBrain));
        }

        public override AIPlayer GetChampion()
        {
            return null;
        }
        #endregion

        protected override void TrainingFinished()
        {
            using (var stream = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.dataPath), this.name + "-accumulatedStats-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture))))
            {
                int gen = 1;

                foreach (var stat in accumulatedStats)
                {
                    if (stat.Item1 != -1)
                        stream.WriteLine("Gen: {0} - {1} {2} {3} {4} {5}", gen, stat.Item1, stat.Item2, stat.Item3, stat.Item4, stat.Item5);
                    else
                        gen++;
                }
            }

            fitnessesRecord.Clear();
            accumulatedStats.Clear();
        }
    }
}