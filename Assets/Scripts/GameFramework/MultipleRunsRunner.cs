using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleRunsRunner : TrainingRunner
{
    [SerializeField]
    [ShowInMenu]
    public AIController attacker;

    [SerializeField]
    [ShowInMenu]
    AIController defender;

    [SerializeField]
    [ShowInMenu]
    int GenCount;

    [SerializeField]
    [ShowInMenu]
    int timesAttackerDefender = 4;

    [SerializeField]
    [ShowInMenu]
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
