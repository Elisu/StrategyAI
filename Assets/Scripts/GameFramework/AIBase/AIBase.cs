using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    public Role role;
    float delay = 10;

    protected Army ownTroops;

    private bool scheduleInProgress = false;

    protected void Start()
    {
        Run();
    }

    public void Run()
    {
        ownTroops = MasterScript.GetArmy(role);
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
        delay -= 1;

        if (delay <= 0 || !scheduleInProgress)
        {
            StartCoroutine(ScheduleFind());
            delay = 100;
        }
            
    }

    private IEnumerator ScheduleFind()
    {
        scheduleInProgress = true;
        for (int i = 0; i < ownTroops.Count; i++)
        {
            FindAction(ownTroops[i]);
            yield return new WaitForSeconds(1);
        }

        scheduleInProgress = false;
    }

    /// <summary>
    /// Override function with implementation of AI action selection
    /// </summary>
    protected virtual void FindAction(IAttack attcker)
    {
        throw new System.NotImplementedException("Your AI must override FindAction method");
    }
}
