using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossTraining : TrainingRunner
{
    [SerializeField]
    AITrainer attacker;
    [SerializeField]
    AITrainer defender;

    AIController starter;

    private void Update()
    {
        if (!TrainingInProgess)
            StartTraining(attacker, defender, 1, 2);
    }
}
