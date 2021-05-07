using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TrainingInstance : Instance
{
    public override bool IsTraining => true;

    public override void Run(IPlayer attack, IPlayer defend)
    {
        SetPlayers(attack, defend);
        IsRunning = true;

        RunGameTrainingLoop();
    }

    private void RunGameTrainingLoop()
    {
        while (defender.OwnArmy.Count != 0 && attacker.OwnArmy.Count != 0)
        {
            if (IsRunning)
            {
                scheduler.ScheduleActions();
                scheduler.RunActions();
            }
        }

        if (defender.OwnArmy.Count == 0)
            Debug.LogWarning("Attacker won");
        else
            Debug.LogWarning("Defender won");

        GameOverHandler();
    }
}
