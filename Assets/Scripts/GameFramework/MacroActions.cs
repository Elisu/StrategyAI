using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroActions
{
    public static bool AttackClosest(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetEnemyArmy(attacker.Side);
        resultAction = null;

        if (army.Count == 0)
            return false;

        IAttack closest = army[0];

        foreach (IAttack troop in army)
        {
            if (Vector2Int.Distance(attacker.Position, closest.Position) > Vector2Int.Distance(attacker.Position, troop.Position))
                closest = troop;
        }

        return AttackGiven(closest, attacker, out resultAction);
    }

    public static bool AttackInRange(Attacker attacker, out IAction resultAction) 
    {
        Army army = attacker.CurrentInstance.GetEnemyArmy(attacker.Side);
        resultAction = null;

        TroopBase inRange = null;

        foreach (TroopBase troop in army)
        {
            if (Mathf.Abs(attacker.Position.x - troop.Position.x) < attacker.Range || Mathf.Abs(attacker.Position.y - troop.Position.y) < attacker.Range)
                inRange = troop;
        }

        if (inRange == null)
            return false;


        return AttackGiven(inRange, attacker, out resultAction);
    }


    public static bool AttackWithLowestHealth(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestHealth, attacker, out resultAction);
    }

    public static bool AttackWithLowestDamage(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestDamage, attacker, out resultAction);
    }

    public static bool AttackWithLowestDefense(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestDefense, attacker, out resultAction);
    }

    private static bool AttackOnCondition(Func<TroopBase> Selection, Attacker attacker, out IAction resultAction)
    {
        TroopBase selected = Selection();
        resultAction = null;

        if (selected == null)
            return false;
        else
            return AttackGiven(selected, attacker, out resultAction);
           
    }

    private static bool Reachable(Attacker attacker, Vector2Int target)
    {
        //Tower cant move and closest target beyond range
        if (attacker is TowerBase && Vector2Int.Distance(attacker.Position, target) > attacker.Range)
            return false;

        if (attacker is TroopBase troop)
            if (troop.FindSpotInRange(target, out Vector2Int inRange))
                if (Pathfinding.FindPath(attacker.Position, inRange, attacker.CurrentInstance) != null)
                     return true;

        return false;
    }

    public static bool AttackGiven(IDamageable target, Attacker attacker, out IAction resultAction)
    {
        resultAction = null;

        if (!Reachable(attacker, target.Position))
            return false;  

        resultAction = new Attack((Damageable)target, attacker);
        return true;
    }

    public static bool MoveToSafety(IMovable runner, out IAction resultAction)
    {
        //TO DO
        resultAction = null;
        return false;
    }

    public static bool DoNothing(Attacker a, out IAction resultAction)
    {
        resultAction = null;
        return true;
    }

}

