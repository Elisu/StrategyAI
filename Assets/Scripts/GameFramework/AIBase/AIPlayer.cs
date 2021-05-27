using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Base class to every AI has to inherit from and implement required methods
/// </summary>
[DataContract]
public abstract class AIPlayer : IPlayer
{
    int currentToSchedule = 0;

    protected internal override Tuple<Attacker, IAction> GetActions()
    {
        for(int i = 0; i < Info.OwnArmy.Count; i++)
        {
            IRecruitable recruit = Info.OwnArmy[currentToSchedule % Info.OwnArmy.Count];
            currentToSchedule++;

            if (recruit is Attacker attack)
            {
                IAction action = FindAction((Attacker)attack);
                return new Tuple<Attacker, IAction>((Attacker)attack, action);                
            }                
        }

        return new Tuple<Attacker, IAction>(null, null);
    }


    /// <summary>
    /// Override function with implementation of AI action selection
    /// </summary>
    /// <param name="attacker">unit which will carry out selected action</param>
    /// <returns>selected action - null means nothing will happen</returns>
    protected internal abstract IAction FindAction(Attacker attacker);

    /// <summary>
    /// Called at the end of a run
    /// </summary>
    protected internal abstract void RunOver(GameStats stats);


    /// <summary>
    /// Implement deepcopy of your AI player
    /// </summary>
    /// <typeparam name="AIType">Your AI class</typeparam>
    /// <returns>deep copy of this instance of your child of AIPlayer</returns>
    public abstract AIPlayer Clone();


}
