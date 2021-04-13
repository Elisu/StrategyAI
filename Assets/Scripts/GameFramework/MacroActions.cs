using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroActions : MonoBehaviour
{
    public static bool AttackClosest(IAttack attacker)
    {
        Army army = MasterScript.GetEnemyArmy(attacker.Side);

        if (army.Count == 0)
            return false;

        IAttack closest = army[0];

        foreach (IAttack troop in army)
        {
            if (Vector2Int.Distance(attacker.Position, closest.Position) > Vector2Int.Distance(attacker.Position, troop.Position))
                closest = troop;
        }

        Debug.LogWarning("Attacking closest");

        return AttackGiven(closest, attacker);
    }

    public static bool AttackInRange(IAttack attacker) 
    {
        Army army = MasterScript.GetEnemyArmy(attacker.Side);

        ITroop inRange = null;

        foreach (ITroop troop in army)
        {
            if (Mathf.Abs(attacker.Position.x - troop.Position.x) < attacker.Range || Mathf.Abs(attacker.Position.y - troop.Position.y) < attacker.Range)
                inRange = troop;
        }

        if (inRange == null)
            return false;

        Debug.LogWarning("Attacking in Range");

        return AttackGiven(inRange, attacker);
    }


    public static bool AttackWithLowestHealth(IAttack attacker)
    {
        Army army = MasterScript.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestHealth, attacker);
    }

    public static bool AttackWithLowestDamage(IAttack attacker)
    {
        Army army = MasterScript.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestDamage, attacker);
    }

    public static bool AttackWithLowestDefense(IAttack attacker)
    {
        Army army = MasterScript.GetArmy(attacker.Side);
        return AttackOnCondition(army.SenseEnemyLowestDefense, attacker);
    }

    private static bool AttackOnCondition(Func<ITroop> Selection, IAttack attacker)
    {
        ITroop selected = Selection();

        if (selected == null)
            return false;
        else
            return AttackGiven(selected, attacker);
           
    }

    private static bool Reachable(IAttack attacker, Vector2Int target)
    {
        //Tower cant move and closest target beyond range
        if (attacker is Tower && Vector2Int.Distance(attacker.Position, target) > attacker.Range)
            return false;

        if (attacker is ITroop troop)
            if (troop.FindSpotInRange(target, out Vector2Int inRange))
                if (Pathfinding.FindPath(attacker.Position, inRange) != null)
                     return true;

        return false;
    }

    public static bool AttackGiven(IDamageable target, IAttack attacker)
    {
        if (!Reachable(attacker, target.Position))
            return false;  

        IAction action = new Attack(target, attacker);
        action.Schedule();

        return true;
    }

    public static bool MoveToSafety(IMovable runner)
    {
        //TO DO
        return false;
    }

    public static bool DoNothing(IMovable a)
    {
        return true;
    }

}
