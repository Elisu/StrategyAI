using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    public Role role;
    float delay = 100;

    protected Army ownTroops;
    protected Army enemyTroops;

    private bool running = false;

    protected void Awake()
    {
        Run();
    }

    public void Run()
    {
        running = true;
        ownTroops = MasterScript.GetArmy(role);
        enemyTroops = MasterScript.GetEnemyArmy(role);
        ownTroops.Add(new Troop<Swordsmen>(10, role));
        ownTroops.Add(new Troop<Swordsmen>(5, role));
        ownTroops.Add(new Troop<Swordsmen>(20, role));
        ownTroops.Add(new Troop<Swordsmen>(5, role));
        ownTroops.Add(new Troop<Swordsmen>(10, role));
        ownTroops.Add(new Troop<Swordsmen>(5, role));
        OnStart();
    }


    /// <summary>
    /// Called at the start of every generation
    /// </summary>
    protected virtual void OnStart()
    {
        return;
    }    
  

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (!running)
            return;

        delay -= 1;

        if (delay <= 0)
        {
            FindAction();
            delay = 100;
        }
            
    }

    /// <summary>
    /// Override function with implementation of AI action selection
    /// </summary>
    protected virtual void FindAction()
    {
        throw new System.NotImplementedException("Your AI must override FindAction method");
    }
}
