﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Scheduler
{
    IPlayer attacker;
    IPlayer defender;

    //Queue<Attacker> inProgress = new Queue<Attacker>();
    Queue<Attacker> inProgress = new Queue<Attacker>();

    public int RunningActionsCount => inProgress.Count;

    public void ScheduleActions()
    {
        Schedule(attacker);
        Schedule(defender);
    }

    private void Schedule(IPlayer player)
    {
        if (player.Info.OwnArmy.Count == 0)
            return;

        Tuple<Attacker, IAction> action = player.GetActions();

        if (action.Item1 == null || action.Item2 == null)
            return;

        //If unit does not have an action set -> set it and add it to running actions
        // else just change the action
        if (action.Item1.Action == null)
        {
            action.Item1.SetAction(action.Item2);
            inProgress.Enqueue(action.Item1);
        }
        else
        {
            action.Item1.SetAction(action.Item2);
        }
    }

    public void RunActions()
    {
        int count = inProgress.Count;

        //Debug.Log(string.Format("Number of running action: {0}", inProgress.Count));
        for (int i = 0; i < count; i++)
        {
            Attacker attacker = inProgress.Dequeue();            

            //if action set and doesn't finish yet -> reschedule again
            if (attacker.Action != null && attacker.Action.Execute())
                inProgress.Enqueue(attacker);

        }
    }

    public void Shopping(Instance instance)
    {
        attacker.Info.OwnArmy.TryBuying(attacker.PickToBuy(), instance);
        defender.Info.OwnArmy.TryBuying(defender.PickToBuy(), instance);
    }

    public void MoneyGain()
    {
        attacker.Info.OwnArmy.MoneyGain();
        defender.Info.OwnArmy.MoneyGain();
    }

    public void Set(IPlayer attacker, IPlayer defender)
    {
        this.attacker = attacker;
        this.defender = defender;

        inProgress.Clear();
        
    }
}
