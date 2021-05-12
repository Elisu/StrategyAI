using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static IRecruitable CreateUnit(Type unit, Role side, Vector2Int spawnPos, Instance instance)
    {
        if (unit.IsSubclassOf(typeof(HumanUnit)))
        {
            var troopType = typeof(Troop<>).MakeGenericType(unit);
            return (IRecruitable)Activator.CreateInstance(troopType, 10, side, spawnPos, instance);
        }
        else
        {
            return null;
        }
    }
}
