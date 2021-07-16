using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerOptions : MonoBehaviour
{
    [SerializeField]
    private List<TrainingRunner> runners;

    private static RunnerOptions instance;

    public static List<TrainingRunner> Options => instance.runners;

    private void Awake()
    {
        instance = this;
    }
}
