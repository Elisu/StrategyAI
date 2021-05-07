using SharpNeat.Phenomes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;
using static Genetic;

public class NeatAI : NeatPlayer
{
    TryAction[] possibleActions;
    Condition[] inputs;
    List<Statistics> stats = new List<Statistics>();

    private void Awake()
    {
        possibleActions = new TryAction[5] { MacroActions.AttackClosest, MacroActions.AttackWithLowestHealth, MacroActions.AttackWithLowestDamage, MacroActions.AttackInRange, MacroActions.DoNothing };
        inputs = new Condition[2] { Conditions.Damaged, Conditions.Free };
    }

    protected override void OnStart()
    {
        //GenerationFinished = false;
    }

    public override float GetFitness()
    {
        GenerationFinished = false;
        if (stats.Count == 0)
            return 0;

        int kills = 0;
        int dealtDamage = 0;
        int receivedDamage = 0;

        foreach (Statistics stat in stats)
        {
            kills += stat.killedEnemies;
            dealtDamage += stat.dealtDamage;
            receivedDamage += stat.receivedDamage;

        }

        float fitness = kills * 100 + dealtDamage * 50 - receivedDamage/2;

        return fitness;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray, Attacker attacker)
    {
        for (int i = 0; i < inputSignalArray.Length; i++)
            inputSignalArray[i] = Convert.ToDouble(inputs[i].Invoke(attacker));
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray, Attacker attacker)
    {
        int bestAction = 0;
        double best = 0;

        for (int i = 0; i < outputSignalArray.Length; i++)
            if (best < outputSignalArray[i])
            {
                best = outputSignalArray[i];
                bestAction = i;
            }

        //possibleActions[bestAction].Invoke(attacker);
    }

    protected override void RunOver()
    {
        GenerationFinished = true;
        List<IRecruitable> dead = OwnArmy.GetDead();

        stats.Clear();
        for (int i = 0; i < dead.Count; i++)
            stats.Add(new Statistics(dead[i].GetStats()));
    }

    public override AIPlayer Clone()
    {
        throw new NotImplementedException();
    }
}
