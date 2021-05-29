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

        private readonly IMacroAction[] outputActions = new IMacroAction[5] { new SerializableMacroActions.AttackClosest(),
                                                                               new SerializableMacroActions.AttackWithLowestHealth(),
                                                                               new SerializableMacroActions.AttackWithLowestDamage(),
                                                                               new SerializableMacroActions.AttackInRange(),
                                                                               new SerializableMacroActions.DoNothing() };

        private readonly ICondition[] inputConditions = new ICondition[] { new Conditions.Damaged(), new Conditions.Free() };

        private int _networkInputCount => inputConditions.Length;
        private int _networkOutputCount => outputActions.Length;

        // Object pooling and Unit management
        private Dictionary<IBlackBox, INeatPlayer> _blackBoxMap = new Dictionary<IBlackBox, INeatPlayer>();
        #endregion

        public NeatEvolutionAlgorithm<NeatGenome, IBlackBox> EvolutionAlgorithm { get; private set; }

        public Experiment Experiment { get; private set; }

        public override Type AIPlayerType => typeof(NeatAI);

        #region UNTIY FUNCTIONS
        protected override void BeforePopCreation()
        {
            LoadExperiment();
        }
        #endregion

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
            EvolutionAlgorithm = Experiment.CreateEvolutionAlgorithm(ExperimentIO.GetSaveFilePath(Experiment.Name, ExperimentFileType.Population));
        }

        protected override List<AIPlayer> CreatPopulation()
        {
            StartEvolution();

            List<INeatPlayer> pop = new List<INeatPlayer>();

            for (int i = 0; i < PopulationSize; i++)
            {
                pop.Add(new NeatAI(outputActions, inputConditions));
            }

            EvolutionAlgorithm.SetPopulation(pop);
            return new List<AIPlayer>(pop);
        }

        protected override void BeforeEachGeneration() => StartCoroutine(EvolutionAlgorithm.PerformOneGeneration());        

        public override void GenerationDone() => EvolutionAlgorithm.Evaluate();     


        protected override void SaveChampion()
        {
            ExperimentIO.WriteChampion(Experiment, EvolutionAlgorithm.CurrentChampGenome);
        }

        public override IPlayer LoadChampion()
        {
            LoadExperiment();
            NeatGenome championBrain = Experiment.LoadChampion();
            return new NeatAI(outputActions, inputConditions, Experiment.CreateGenomeDecoder().Decode(championBrain));
        }

        public override AIPlayer GetChampion()
        {
            return null;
        }
        #endregion
    }
}