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

        //Tower cant move and closest target beyond range
        if (closest is Tower && Vector2Int.Distance(closest.Position, attacker.Position) > attacker.Range)
            return false;

        Debug.LogWarning("Attacking closest");

        attacker.StopAction();

        if (attacker.Side == Role.Attacker)
            Scheduler.Attacker.Enqueue(new Attack(closest, attacker));
        else
            Scheduler.Defender.Enqueue(new Attack(closest, attacker));

        return true;
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

        attacker.StopAction();

        if (attacker.Side == Role.Attacker)
            Scheduler.Attacker.Enqueue(new Attack(inRange, attacker));
        else
            Scheduler.Defender.Enqueue(new Attack(inRange, attacker));

        return true;
    }


    public static bool AttackWithLowestHealth(IAttack attacker)
    {
        Army army = MasterScript.GetEnemyArmy(attacker.Side);
        return AttackOnCondition(army.GetTroopLowestHealth, attacker);
    }

    public static bool AttackWithLowestDamage(IAttack attacker)
    {
        Army army = MasterScript.GetEnemyArmy(attacker.Side);
        return AttackOnCondition(army.GetTroopLowestDamge, attacker);
    }

    public static bool AttackWithLowestDefense(IAttack attacker)
    {
        Army army = MasterScript.GetEnemyArmy(attacker.Side);
        return AttackOnCondition(army.GetTroopLowestDefense, attacker);
    }

    private static bool AttackOnCondition(Func<ITroop> Selection, IAttack attacker)
    {
        ITroop selected = Selection();

        if (selected == null)
            return false;
        else
        {
            attacker.StopAction();

            if (attacker.Side == Role.Attacker)
                Scheduler.Attacker.Enqueue(new Attack(selected, attacker));
            else
                Scheduler.Defender.Enqueue(new Attack(selected, attacker));

            return true;
        }       
    }

    public static bool AttackGiven(IDamageable target, IAttack attacker)
    {
        Debug.LogWarning("Attacking given");

        attacker.StopAction();

        if (attacker.Side == Role.Attacker)
            Scheduler.Attacker.Enqueue(new Attack(target, attacker));
        else
            Scheduler.Defender.Enqueue(new Attack(target, attacker));

        return true;
    }

    public static bool MoveToSafety(IMovable runner)
    {
        //TO DO
        return false;
    }

}

