using SharpNeat.Phenomes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;
using Genetic;
using System.Runtime.Serialization;
using System.Collections.Concurrent;

public class NeatAI : INeatPlayer
{
    IMacroAction[] possibleActions;
    ICondition[] inputs;
    ConcurrentQueue<GameStats> accumulatedResults;

    public NeatAI(IMacroAction[] actions, ICondition[] conditions, IBlackBox brain) : base(brain)
    {
        accumulatedResults = new ConcurrentQueue<GameStats>();
        possibleActions = actions;
        inputs = conditions;
    }

    private NeatAI(IMacroAction[] actions, ICondition[] conditions, ConcurrentQueue<GameStats> result, IBlackBox brain) : base(brain)
    {
        possibleActions = actions;
        inputs = conditions;
        accumulatedResults = result;
    }


    public override float GetFitness()
    {
        return EvolutionFunctions.ComputeFitness(accumulatedResults.ToArray()[0], Side).Item1;
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray, Attacker attacker)
    {
        for (int i = 0; i < inputSignalArray.Length; i++)
            inputSignalArray[i] = Convert.ToDouble(inputs[i].Evaluate(attacker, Info));
    }

    protected override IAction UseBlackBoxOutpts(ISignalArray outputSignalArray, Attacker attacker)
    {
        IAction resultAction = null;

        int bestAction = 0;
        double best = 0;

        for (int i = 0; i < outputSignalArray.Length; i++)
            if (best < outputSignalArray[i])
            {
                best = outputSignalArray[i];
                bestAction = i;
            }

        possibleActions[bestAction].TryAction(attacker, out resultAction);

        return resultAction;
    }

    protected override void RunOver(GameStats stats)
    {
        accumulatedResults.Enqueue(stats);
    }

    public override AIPlayer Clone()
    {
        return new NeatAI(possibleActions, inputs, accumulatedResults, blackBox.Clone());
    }

    protected override int PickToBuy()
    {
        return UnitFinder.PickOnBudget(Info.OwnArmy.Money);
    }
}
