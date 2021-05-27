using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Move : IAction
{
    Vector2Int target;
    TroopBase troop;

    public Move(int x, int y, TroopBase troop)
    {
        target = new Vector2Int(x, y);
        this.troop = troop;
    }

    public Move(float x, float y, TroopBase troop)
    {
        target = new Vector2Int((int)x/5, (int)y/5);
        this.troop = troop;
    }

    public Move(Vector2Int targetPos, TroopBase troop)
    {
        target = targetPos;
        this.troop = troop;
    }

    public bool Execute()
    {
        if (troop.Health <= 0)
        {
            troop.StopAction();
            return false;
        }
        
        bool stillGoing = troop.MoveTo(target);

        if (!stillGoing)
            troop.StopAction();

        return stillGoing;
    }
}
