using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraningLoop : MonoBehaviour
{
    public AIBase attacker;
    public AIBase defender;

    public int GenerationCount;


    private void OnEnable()
    {
        MasterScript.GameOver += Run;
    }

    private void OnDisable()
    {
        MasterScript.GameOver -= Run;
    }

    void Run()
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
