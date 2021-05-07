using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

internal class GameInstance : Instance
{
    public override bool IsTraining => false;

    public override void Run(IPlayer attack, IPlayer defend)
    {
        SetPlayers(attack, defend);
        IsRunning = true;
    }

    private void FixedUpdate()
    {
        if (IsRunning)
        {
            RunGameStep();
        }
        else
        {
            if (defender.OwnArmy.Count == 0)
                Debug.Log("Attacker won");
            else
                Debug.Log("Defender won");
        }
        
    }

    protected void RunGameStep()
    {
        if (defender.OwnArmy.Count == 0 || attacker.OwnArmy.Count == 0)
            IsRunning = false;

        scheduler.ScheduleActions();
        scheduler.RunActions();
    }

}

