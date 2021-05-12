using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to every AI has to inherit from and implement required methods
/// </summary>
public abstract class AIPlayer : IPlayer
{
    public virtual int RunsPerGenerations => runsPerGen;

    protected int runsPerGen = 1;

    int currentToSchedule = 0;

    public void Run()
    {
        
        OwnArmy.ClearGraveyard();
        OnStart();
    }


    /// <summary>
    /// Called at the start of every generation
    /// </summary>
    protected virtual void OnStart()
    {
        return;
    } 
    

    protected internal override Tuple<Attacker, IAction> GetActions()
    {
        Attacker attack = OwnArmy[currentToSchedule % OwnArmy.Count];
        currentToSchedule++;

        IAction action = FindAction(attack);
        return new Tuple<Attacker, IAction>(attack, action);
    }


    /// <summary>
    /// Override function with implementation of AI action selection
    /// </summary>
    /// <param name="attacker">unit which will carry out selected action</param>
    /// <returns>selected action - null means nothing will happen</returns>
    protected internal abstract IAction FindAction(Attacker attacker);


    /// <summary>
    /// Implement deepcopy of your AI player
    /// </summary>
    /// <typeparam name="AIType">Your AI class</typeparam>
    /// <returns>deep copy of this instance of your child of AIPlayer</returns>
    public abstract AIPlayer Clone();

}
