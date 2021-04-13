﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Scheduler : MonoBehaviour
{
    public static Queue<IAction> Attacker = new Queue<IAction>();
    public static Queue<IAction> Defender = new Queue<IAction>();

    private void OnEnable()
    {
        MasterScript.GameOver += Restart;
    }

    private void OnDisable()
    {
        MasterScript.GameOver -= Restart;
    }

    private void FixedUpdate()
    {
       if (Attacker.Count != 0)
       {
            Debug.Log("Attacker action");
            IAction action = Attacker.Dequeue();
            action.Start();
       }    

       if (Defender.Count != 0)
       {
            Debug.Log("Defender action");
            IAction action = Defender.Dequeue();
            action.Start();
        }      
            
    }

    private void Restart()
    {
        Attacker.Clear();
        Defender.Clear();
    }
}