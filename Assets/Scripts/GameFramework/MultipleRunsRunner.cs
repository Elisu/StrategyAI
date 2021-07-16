using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleRunsRunner : TrainingRunner
{
    [SerializeField]
    public AIController attacker;

    [SerializeField]
    AIController defender;

    [SerializeField]
    int GenCount;

    [SerializeField]
    int timesAttackerDefender = 4;

    [SerializeField]
    int timesDefenderAttacker = 4;

    protected override void OnUpdate()
    {
        if (!TrainingInProgess && timesAttackerDefender > 0)
        {
            StartTraining(attacker, defender, 3, GenCount);
            timesAttackerDefender--;
        }            

        if (!TrainingInProgess && timesAttackerDefender < 1 && timesDefenderAttacker > 0)
        {
            StartTraining(defender, attacker, 3, GenCount);
            timesDefenderAttacker--;
        }


    }
}
