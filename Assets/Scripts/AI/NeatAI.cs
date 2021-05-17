using SharpNeat.Phenomes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;
using static Genetic;

public class NeatAI : INeatPlayer
{
    TryAction[] possibleActions;
    Condition[] inputs;
    GameStats stats;

    public NeatAI(TryAction[] actions, Condition[] conditions)
    {
        possibleActions = actions;
        inputs = conditions;
    }


    public override float GetFitness()
    {
        List<Statistics> myStats = stats.GetMyStats(role);

        int kills = 0;
        int dealtDamage = 0;
        int receivedDamage = 0;
        float fitness = 0;

        if (stats.Winner == role)
            fitness += 10000;

        foreach (Statistics stat in myStats)
        {
            kills += stat.killedEnemies;
            dealtDamage += stat.dealtDamage;
            receivedDamage += stat.receivedDamage;

        }

        fitness += kills * 1000 + dealtDamage;
        return fitness;
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray, Attacker attacker)
    {
        for (int i = 0; i < inputSignalArray.Length; i++)
            inputSignalArray[i] = Convert.ToDouble(inputs[i].Invoke(attacker));
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

        possibleActions[bestAction].Invoke(attacker, out resultAction);

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
        return UnitFinder.PickOnBudget(OwnArmy.Money);
    }
}
