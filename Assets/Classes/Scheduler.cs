using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    public static Queue<IAction> Attacker = new Queue<IAction>();
    public static Queue<IAction> Defender = new Queue<IAction>();

    private void FixedUpdate()
    {
       if (Attacker.Count != 0)
       {
            Debug.Log("Attacker action");
            IAction action = Attacker.Dequeue();
            action.Execute();
       }    

       if (Defender.Count != 0)
       {
            Debug.Log("Defender action");
            IAction action = Defender.Dequeue();
            action.Execute();
        }      
            
    }
}
