using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRunner : TrainingRunner
{
    [SerializeField]
    [ShowInMenu]
    AIController attacker;
    [SerializeField]
    [ShowInMenu]
    AIController defender;
    [SerializeField]
    [ShowInMenu]
    int tryCount;
    [SerializeField]
    [ShowInMenu]
    int generationCount;
    [SerializeField]
    string attackerSave;
    [SerializeField]
    string defenderSave;

    protected override void OnStart()
    {
        StartTraining(attacker, defender, tryCount, generationCount, attackerSave, defenderSave);
    }
}
