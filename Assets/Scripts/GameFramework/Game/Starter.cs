using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    private void Awake()
    {
        var runners = FindObjectsOfType<TrainingRunner>();

        foreach (var runner in runners)
            if (runner != null && runner.isActiveAndEnabled)
            {
                runner.OnAwake();
                return;
            }
                
    }
}
