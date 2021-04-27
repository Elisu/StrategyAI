using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TraningLoop : MonoBehaviour
{
    public AIPlayer attacker;
    public AIPlayer defender;

    public int gameInstancesCount;
    public GameInstance gameInstance;

    public int GenerationCount;

    public void Start()
    {
        for (int i = 0; i < gameInstancesCount; i++)
            Instantiate(gameInstance);
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
