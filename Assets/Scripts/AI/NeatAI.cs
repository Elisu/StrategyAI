using SharpNeat.Phenomes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;
using Genetic;
using System.Runtime.Serialization;

public class NeatAI : INeatPlayer
{
    IMacroAction[] possibleActions;
    ICondition[] inputs;
    GameStats stats;

    public NeatAI(IMacroAction[] actions, ICondition[] conditions, IBlackBox brain = null)
    {
        possibleActions = actions;
        inputs = conditions;
        SetBlackBox(brain);
    }

    public override float GetFitness()
    {
        return EvolutionFunctions.ComputeFitness(stats, Side);
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
        this.stats = stats;
    }

    public override AIPlayer Clone()
    {
        return this;
    }

    protected override int PickToBuy()
    {
        return UnitFinder.PickOnBudget(Info.OwnArmy.Money);
    }
}
