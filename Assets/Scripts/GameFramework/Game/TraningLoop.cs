using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TraningLoop : MonoBehaviour
{
    public AIBase attacker;
    public AIBase defender;

    public int GenerationCount;

    public void Start()
    {
        GenerationCount *= Mathf.Max(attacker.RunsPerGenerations, defender.RunsPerGenerations);
    }


    internal void Run()
    {
        //One Run already happened 
        GenerationCount--;

        if (GenerationCount > 0)
        {
            attacker.Run();
            defender.Run();
        }
    }


}
