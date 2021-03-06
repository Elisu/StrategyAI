using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : IAction
{
    Vector2Int target;
    ITroop troop;

    public Move(int x, int y, ITroop troop)
    {
        target = new Vector2Int(x, y);
        this.troop = troop;
    }

    public Move(float x, float y, ITroop troop)
    {
        target = new Vector2Int((int)x/5, (int)y/5);
        this.troop = troop;
    }

    public void Execute()
    {
        Debug.Log("Executing Move");
        List<Vector2Int> path = Pathfinding.FindPath(troop.Position, target);
        MasterScript.moving.Enqueue(new Tuple<IMovable, Queue<Vector2Int>>(troop, new Queue<Vector2Int>(path)));
    }
}
