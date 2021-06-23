using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRunner : TrainingRunner
{
    [SerializeField]
    AIController attacker;
    [SerializeField]
    AIController defender;
    [SerializeField]
    int tryCount;
    [SerializeField]
    int generationCount;
    [SerializeField]
    string attackerSave;
    [SerializeField]
    string defenderSave;

    public void Start()
    {
        StartTraining(attacker, defender, tryCount, generationCount, attackerSave, defenderSave);
    }
}
