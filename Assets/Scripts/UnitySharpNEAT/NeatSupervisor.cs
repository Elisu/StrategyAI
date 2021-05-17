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
using static Genetic;

namespace UnitySharpNEAT
{
    /// <summary>
    /// This class acts as the entry point for the NEAT evolution.
    /// It manages the UnitController's being evolved and handles the creation of the NeatEvolutionAlgorithm.
    /// It is also responsible for managing the lifecycle of the evolution, e.g. by starting/stopping it.
    /// </summary>
    public class NeatSupervisor : AITrainer
    {
        #region FIELDS
        [Header("Experiment Settings")]

        [SerializeField]
        private string _experimentConfigFileName = "experiment.config";

        [SerializeField]
        private int _networkInputCount = 5;

        [SerializeField]
        private int _networkOutputCount = 2;

        public int PopulationSize;
  


        [Header("Evaluation Settings")]

        [Tooltip("How many times per generation the generation gets evaluated.")]
        public int Trials = 1;

        public bool GenerationFinished = false;

        [Header("Debug")]

        [SerializeField]
        private bool _enableDebugLogging = false;


        // Object pooling and Unit management
        private Dictionary<IBlackBox, INeatPlayer> _blackBoxMap = new Dictionary<IBlackBox, INeatPlayer>();

        private DateTime _startTime;
        #endregion

        #region PROPERTIES
        public int NetworkInputCount { get => _networkInputCount; }

        public int NetworkOutputCount { get => _networkOutputCount; }

        public uint CurrentGeneration { get; private set; }

        public double CurrentBestFitness { get; private set; }

        public NeatEvolutionAlgorithm<NeatGenome, IBlackBox> EvolutionAlgorithm { get; private set; }

        public Experiment Experiment { get; private set; }

        #endregion

        #region UNTIY FUNCTIONS
        protected override void BeforePopCreation()
        {
            Utility.DebugLog = _enableDebugLogging;

            // load experiment config file and use it to create an Experiment
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
        #endregion

        #region NEAT LIFECYCLE
        /// <summary>
        /// Starts the NEAT algorithm.
        /// </summary>
        public void StartEvolution()
        {
            if (EvolutionAlgorithm != null && EvolutionAlgorithm.RunState == SharpNeat.Core.RunState.Running)
                return;

            //DeactivateAllUnits();

            Utility.Log("Starting Experiment.");
            _startTime = DateTime.Now;

            EvolutionAlgorithm = Experiment.CreateEvolutionAlgorithm(ExperimentIO.GetSaveFilePath(Experiment.Name, ExperimentFileType.Population));
            EvolutionAlgorithm.UpdateEvent += new EventHandler(HandleUpdateEvent);

            //StartCoroutine(EvolutionAlgorithm.PerformOneGeneration());
        }

        #endregion
       

        #region EVENT HANDLER
        /// <summary>
        /// Event callback which gets called at the end of each generation.
        /// </summary>
        void HandleUpdateEvent(object sender, EventArgs e)
        {
            Utility.Log(string.Format("Generation={0:N0} BestFitness={1:N6}", EvolutionAlgorithm.CurrentGeneration, EvolutionAlgorithm.Statistics._maxFitness));

            CurrentBestFitness = EvolutionAlgorithm.Statistics._maxFitness;
            CurrentGeneration = EvolutionAlgorithm.CurrentGeneration;
        }

        protected override List<AIPlayer> CreatPopulation()
        {
            StartEvolution();

            var possibleActions = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
            var inputs = new Condition[2] { Conditions.Damaged, Conditions.Free };

            List<INeatPlayer> pop = new List<INeatPlayer>();

            for (int i = 0; i < PopulationSize; i++)
            {
                pop.Add(new NeatAI(possibleActions, inputs));
            }

            EvolutionAlgorithm.SetPopulation(pop);
            return new List<AIPlayer>(pop);
        }

        protected override void BeforeEachGeneration()
        {
            EvolutionAlgorithm.PerformOneGeneration();
        }

        public override void GenerationDone()
        {
            EvolutionAlgorithm.Evaluate();
        }

        public override AIPlayer GetRepresentative()
        {
            return null;
        }
        #endregion
    }
}