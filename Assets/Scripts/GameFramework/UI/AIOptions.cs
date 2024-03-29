using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOptions : MonoBehaviour
{
    [SerializeField]
    private List<AIController> AIs;

    private static AIOptions instance;

    public static List<AIController> Options => instance.AIs;

    private void Awake()
    {
        instance = this;
    }
}
