using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIPlayer : IPlayer
{
    public virtual int RunsPerGenerations => runsPerGen;
    float delay = 10;

    protected int runsPerGen = 1;
    private bool scheduleInProgress = false;
    private bool running;

    protected void Start()
    {
        Run();
    }

    protected void OnEnable()
    {
        MasterScript.GameOver += RunOverHandler;
    }

    protected void OnDisable()
    {
        MasterScript.GameOver -= RunOverHandler;
    }

    public void Run()
    {
        ownTroops = MasterScript.GetArmy(role);
        ownTroops.Add(new Troop<Swordsmen>(50, role));
        ownTroops.Add(new Troop<Swordsmen>(30, role));
        //ownTroops.Add(new Troop<Cavalry>(10, role));
        ownTroops.ClearGraveyard();
        running = true;
        OnStart();
    }


    /// <summary>
    /// Called at the start of every generation
    /// </summary>
    protected virtual void OnStart()
    {
        return;
    } 
    
    /// <summary>
    /// Called at the end of a run
    /// </summary>
    protected virtual void RunOver()
    {
        return;
    }
  

    protected void FixedUpdate()
    {
        if (!running)
            return;

        delay -= 1;

        if (delay <= 0 || !scheduleInProgress )
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

    private void RunOverHandler()
    {
        running = false;
        RunOver();
    }

    /// <summary>
    /// Override function with implementation of AI action selection
    /// </summary>
    protected virtual void FindAction(Attacker attcker)
    {
        throw new System.NotImplementedException("Your AI must override FindAction method");
    }
}
