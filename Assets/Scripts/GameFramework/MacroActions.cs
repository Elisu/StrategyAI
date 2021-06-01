using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class MacroActions
{
    public static bool AttackClosest(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetEnemyArmy(attacker.Side);
        resultAction = null;

        if (army.Count == 0)
            return false;

        IRecruitable closest = army[0];

        foreach (IRecruitable troop in army)
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

        IRecruitable inRange = null;

        foreach (IRecruitable troop in army)
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
        Army army = attacker.CurrentInstance.GetEnemyArmy(attacker.Side);
        return AttackOnCondition(army.SenseLowestHealth, attacker, out resultAction);
    }

    public static bool AttackWithLowestDamage(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetEnemyArmy(attacker.Side);
        return AttackOnCondition(army.SenseTroopLowestDamage, attacker, out resultAction);
    }

    public static bool AttackWeakestAgainst(Attacker attacker, out IAction resultAction)
    {
        Army army = attacker.CurrentInstance.GetEnemyArmy(attacker.Side);
        return AttackOnCondition(army.SenseTroopLowestDamage, attacker, out resultAction);
    }

    private static bool AttackOnCondition(Func<IRecruitable> Selection, Attacker attacker, out IAction resultAction)
    {
        IRecruitable selected = Selection();
        resultAction = null;

        if (selected == null)
            return false;
        else
            return AttackGiven(selected, attacker, out resultAction);
           
    }

    private static bool Reachable(Attacker attacker, Vector2Int target)
    {
        //Tower cant move and closest target beyond range
        if (attacker is TowerBase && Vector2Int.Distance(attacker.Position, target) <= attacker.Range)
            return true;

        if (attacker is TroopBase troop)
            if (troop.FindSpotInRange(target, out Vector2Int inRange))
                if (Pathfinding.FindPath(attacker.Position, inRange, attacker.Side, attacker.CurrentInstance) != null)
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

public class SerializableMacroActions
{
    [DataContract]
    public class AttackClosest : IMacroAction
    {
        public override bool TryAction(Attacker a, out IAction resultAction)
        {
            return MacroActions.AttackClosest(a, out resultAction);
        }
    }

    [DataContract]
    public class AttackInRange : IMacroAction
    {
        public override bool TryAction(Attacker a, out IAction resultAction)
        {
            return MacroActions.AttackInRange(a, out resultAction);
        }
    }

    [DataContract]
    public class AttackWithLowestHealth : IMacroAction
    {
        public override bool TryAction(Attacker a, out IAction resultAction)
        {
            return MacroActions.AttackWithLowestHealth(a, out resultAction);
        }
    }

    [DataContract]
    public class AttackWithLowestDamage : IMacroAction
    {
        public override bool TryAction(Attacker a, out IAction resultAction)
        {
            return MacroActions.AttackWithLowestDamage(a, out resultAction);
        }
    }

    [DataContract]
    public class DoNothing : IMacroAction
    {
        public override bool TryAction(Attacker a, out IAction resultAction)
        {
            return MacroActions.DoNothing(a, out resultAction);
        }
    }
}

[DataContract]
[KnownType(typeof(SerializableMacroActions.AttackClosest))]
[KnownType(typeof(SerializableMacroActions.AttackInRange))]
[KnownType(typeof(SerializableMacroActions.AttackWithLowestHealth))]
[KnownType(typeof(SerializableMacroActions.AttackWithLowestDamage))]
[KnownType(typeof(SerializableMacroActions.DoNothing))]
public abstract class IMacroAction
{
    public abstract bool TryAction(Attacker a, out IAction resultAction);
}

