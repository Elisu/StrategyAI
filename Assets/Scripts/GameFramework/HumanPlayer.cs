using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : IPlayer
{
    public Queue<int> toBuy;
    public Queue<Tuple<Attacker, IAction>> toDo;

    protected internal override Tuple<Attacker, IAction> GetActions()
    {
        if (toDo.Count == 0)
            return new Tuple<Attacker, IAction>(null, null);

        return toDo.Dequeue();
    }

    protected internal override int PickToBuy()
    {
        if (toBuy.Count == 0)
            return 0;

        return toBuy.Dequeue();
    }

}
