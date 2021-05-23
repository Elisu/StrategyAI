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
            return (IRecruitable)Activator.CreateInstance(troopType, side, spawnPos, instance);
        }
        else
        {
            return null;
        }
    }

    public static IRecruitable CreateStructure(string tag, Vector2Int position, Instance instance)
    {
        switch (tag)
        {
            case "Tower":
                return new Tower<BasicTower>(position, instance);
            case "Gate":
                return new Gate(position, instance);
            case "Wall":
                return new Wall(position, instance);
        }

        return null;
    }

    public static IRecruitable CreateStructure(Transform structure, Vector2Int position, Instance instance)
    {
        switch (structure.tag)
        {
            case "Tower":
                return new Tower<BasicTower>(position, instance, structure.gameObject);
            case "Gate":
                return new Gate(position, instance, structure.gameObject);
            case "Wall":
                return new Wall(position, instance, structure.gameObject);
        }

        return null;
    }
}
