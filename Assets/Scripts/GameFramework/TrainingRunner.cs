using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingRunner : MonoBehaviour
{
    [SerializeField]
    private TrainingLoop training;

    protected bool TrainingInProgess => training.TrainingInProgress;

    private void Awake()
    {
        training = Instantiate(training);
    }

    protected bool StartTraining(AIController attack, AIController defend, int tryCount, int genCount, string attackSave = null, string defendSave = null)
    {
        if (TrainingInProgess)
            return false;

        training.StartTraining(attack, defend, tryCount, genCount, attackSave, defendSave);
        return true;
    }
}
