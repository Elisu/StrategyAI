using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroActions : MonoBehaviour
{
    public static bool AttackClosest(Attacker attacker)
    {
        Army army = attacker.Instance.GetEnemyArmy(attacker.Side);

        if (army.Count == 0)
            return false;

        IAttack closest = army[0];

        foreach (IAttack troop in army)
        {
            if (Vector2Int.Distance(attacker.Position, closest.Position) > Vector2Int.Distance(attacker.Position, troop.Position))
                closest = troop;
        }

        return AttackGiven(closest, attacker);
    }

    public static bool AttackInRange(Attacker attacker) 
    {
        Army army = attacker.Instance.GetEnemyArmy(attacker.Side);

        TroopBase inRange = null;

        foreach (TroopBase troop in army)
        {
            if (Mathf.Abs(attacker.Position.x - troop.Position.x) < attacker.Range || Mathf.Abs(attacker.Position.y - troop.Position.y) < attacker.Range)
                inRange = troop;
        }

        if (inRange == null)
            return false;


        return AttackGiven(inRange, attacker);
    }


    public static bool AttackWithLowestHealth(Attacker attacker)
    {
        Army army = attacker.Instance.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestHealth, attacker);
    }

    public static bool AttackWithLowestDamage(Attacker attacker)
    {
        Army army = attacker.Instance.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestDamage, attacker);
    }

    public static bool AttackWithLowestDefense(Attacker attacker)
    {
        Army army = attacker.Instance.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestDefense, attacker);
    }

    private static bool AttackOnCondition(Func<TroopBase> Selection, Attacker attacker)
    {
        TroopBase selected = Selection();

        if (selected == null)
            return false;
        else
            return AttackGiven(selected, attacker);
           
    }

    private static bool Reachable(Attacker attacker, Vector2Int target)
    {
        //Tower cant move and closest target beyond range
        if (attacker is TowerBase && Vector2Int.Distance(attacker.Position, target) > attacker.Range)
            return false;

        if (attacker is TroopBase troop)
            if (troop.FindSpotInRange(target, out Vector2Int inRange))
                if (Pathfinding.FindPath(attacker.Position, inRange, attacker.Instance) != null)
                     return true;

        return false;
    }

    public static bool AttackGiven(IDamageable target, Attacker attacker)
    {
        if (!Reachable(attacker, target.Position))
            return false;  

        IAction action = new Attack((Damageable)target, attacker);
        action.Schedule();

        return true;
    }

    public static bool MoveToSafety(IMovable runner)
    {
        //TO DO
        return false;
    }

    public static bool DoNothing(Attacker a)
    {
        return true;
    }

}

