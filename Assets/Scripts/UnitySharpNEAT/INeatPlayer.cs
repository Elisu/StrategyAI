/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using SharpNeat.Phenomes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace UnitySharpNEAT
{
    /// <summary>
    /// Abstract representation of a Unit, which is equipped with a Neural Net (IBlackBox).
    /// The IBlackBox gets fed with inputs and computes an output, which can be used to control the Unit.
    /// </summary>
    [DataContract]
    public abstract class INeatPlayer : AIPlayer
    {
        [DataMember]
        public IBlackBox BlackBox { get; private set; }

        protected override IAction FindAction(Attacker attacker)
        {
            // feed the black box with input
            UpdateBlackBoxInputs(BlackBox.InputSignalArray, attacker);

            // calculate the outputs
            BlackBox.Activate();

            // do something with those outputs
            return UseBlackBoxOutpts(BlackBox.OutputSignalArray, attacker);

        }

        public void SetBlackBox(IBlackBox brain)
        {
            if (BlackBox == null)
                BlackBox = brain;
        }

        /// <summary>
        /// Feed the BlackBox with inputs.
        /// Do that by modifying its input signal array.
        /// The size of the array corresponds to NeatSupervisor.NetworkInputCount
        /// </summary>
        protected abstract void UpdateBlackBoxInputs(ISignalArray inputSignalArray, Attacker attacker);

        /// <summary>
        /// Do something with the computed outputs of the BlackBox.
        /// The size of the array corresponds to NeatSupervisor.NetworkOutputCount
        /// </summary>
        protected abstract IAction UseBlackBoxOutpts(ISignalArray outputSignalArray, Attacker attacker);

        /// <summary>
        /// Called during the evaluation phase (at the end of each trail). 
        /// The performance of this unit, i.e. it's fitness, is retrieved by this function.
        /// Implement a meaningful fitness function here.
        /// </summary>
        public abstract float GetFitness();

    }
}
