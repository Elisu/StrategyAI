using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class AIController : IPlayerController
{
    public virtual ReadOnlyCollection<AIPlayer> Population { get; private set; }

    public abstract Type AIPlayerType { get; }

    protected internal virtual void OnStart()
    {
        InitializeVariables();
        CustomStart();
        Population = Array.AsReadOnly(new AIPlayer[1] { (AIPlayer)GetPlayer() });
        BeforeEachGeneration();
    }

    private void InitializeVariables()
    {
        Type t = this.GetType();
    }

    protected virtual void CustomStart()
    {
        return;
    }

    /// <summary>
    /// Method called before the start of each generation - override if needed
    /// </summary>
    protected internal virtual void BeforeEachGeneration()
    {
        return;
    }

    protected internal virtual void TrainingFinished()
    {
        return;
    }


}
