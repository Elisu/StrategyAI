using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingRunner : MonoBehaviour
{
    [SerializeField]
    private TrainingLoop training;

    protected bool TrainingInProgess => training.TrainingInProgress;

    internal void OnAwake()
    {
        training = Instantiate(training);
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnStart()
    {
        return;
    }

    protected virtual void OnUpdate()
    {
        return;
    }

    protected bool StartTraining(AIController attack, AIController defend, int tryCount, int genCount, string attackSave = null, string defendSave = null)
    {
        if (TrainingInProgess)
            return false;

        var controllers = FindObjectsOfType<AIController>();

        foreach (var AI in controllers)
            if (AI.gameObject.tag == Role.Attacker.ToString())
                attack = AI;
            else
                defend = AI;


        training.StartTraining(attack, defend, tryCount, genCount, attackSave, defendSave);
        return true;
    }
}
