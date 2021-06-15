using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class HumanPlayer : IPlayer
{
    public Queue<int> toBuy = new Queue<int>();
    public Queue<Tuple<Attacker, IAction>> toDo = new Queue<Tuple<Attacker, IAction>>();

    public int Money => Info.OwnArmy.Money;

    public HumanPlayer()
    {
        toBuy.Enqueue(0);
    }

    protected internal override Tuple<Attacker, IAction> GetActions()
    {
        if (toDo.Count == 0)
        {
            //Auto attack for human player
            foreach (var recruit in Info.OwnArmy)
                if (recruit is Attacker attacker && recruit.CurrentState == State.Free)
                {
                    MacroActions.AttackInRange(attacker, out IAction result);
                    return new Tuple<Attacker, IAction>(attacker, result);
                }

            return new Tuple<Attacker, IAction>(null, null);
        }
           

        return toDo.Dequeue();
    }

    protected internal override int PickToBuy()
    {
        if (toBuy.Count == 0)
            return -1;

        return toBuy.Dequeue();
    }

    internal void SetAction(Attacker recruit, IAction action)
    {
        toDo.Enqueue(new Tuple<Attacker, IAction>(recruit, action));
    }

    internal void SetToBuy(int unitIndex)
    {
        toBuy.Enqueue(unitIndex);
    }

}
