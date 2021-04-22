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
        Debug.Log("Executing Move");
        troop.MoveForAttack(target);
        bool stillGoing = troop.Move();

        if (!stillGoing)
            troop.StopAction();

        return stillGoing;
    }

    public void Schedule()
    {
        throw new NotImplementedException();
    }

    public void Start()
    {
        throw new NotImplementedException();
    }
}
