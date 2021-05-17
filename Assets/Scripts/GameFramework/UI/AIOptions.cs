using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOptions : MonoBehaviour
{
    [SerializeField]
    private List<AITrainer> AIs;

    private static AIOptions instance;

    public static List<AITrainer> Options => instance.AIs;

    private void Awake()
    {
        instance = this;
    }
}
