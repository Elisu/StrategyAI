using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

internal class GameInstance : Instance
{
    public override bool IsTraining => false;

    public Role winner { get; private set; }

    public void Run(IPlayer attack, IPlayer defend)
    {
        SetPlayers(attack, defend);
        IsRunning = true;
        scheduler.Shopping(this);
        scheduler.Shopping(this);
        scheduler.Shopping(this);
    }

    private void FixedUpdate()
    {
        if (IsRunning)
        {
            RunGameStep();
        }        
    }

    protected void RunGameStep()
    {
        if (defender.Info.OwnArmy.Troops.Count == 0 || attacker.Info.OwnArmy.Troops.Count == 0)
        {
            if (defender.Info.OwnArmy.Troops.Count == 0)
                winner = Role.Attacker;
            else
                winner = Role.Defender;

            IsRunning = false;
            GameOverHandler();
        }
            

        scheduler.Shopping(this);
        scheduler.ScheduleActions();
        scheduler.RunActions();
    }

}

