using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : IEnumerable<Attacker>
{
    public Role Side { get; private set; }

    public int Count => troops.Count + towers.Count;

    public int Money { get; private set; }

    private List<TroopBase> troops = new List<TroopBase>();
    private List<Building> buildings = new List<Building>();
    private List<TowerBase> towers = new List<TowerBase>();
    private Army enemyTroops;

    private List<IRecruitable> graveyard = new List<IRecruitable>();

    public Army(Role side)
    {
        Side = side;
        Money = 10000;
    }

    internal void SetEnemy(Instance instance)
    {
        enemyTroops = instance.GetEnemyArmy(Side);
    }

    internal IRecruitable AddStructure(string tag, Vector2Int position, Instance instance)
    {
        IRecruitable structure = UnitFactory.CreateStructure(tag, position, instance);
        Add(structure);
        return structure;
    }

    internal IRecruitable AddStructure(Transform structureObject, Vector2Int position, Instance instance)
    {
        IRecruitable structure = UnitFactory.CreateStructure(structureObject, position, instance);
        Add(structure);
        return structure;
    }

    internal bool TryBuying(int toBuy, Instance instance)
    {
        Type unitType = UnitFinder.unitTypes[toBuy];
        int price = UnitFinder.unitStats[toBuy].Price;

        //Attacker can't buy towers
        if (Side == Role.Attacker && !(unitType.IsSubclassOf(typeof(HumanUnit))))
            return false;

        if (price <= Money && instance.Map.GetFreeSpawn(Side, out Vector2Int spawnPos))
        {
            Money -= price;

            
            Add(UnitFactory.CreateUnit(unitType, Side, spawnPos, instance));
            return true;
        }

        return false;
    }

    public void MoneyGain()
    {
        if (Side == Role.Defender)
            Money += 5;
        else
            Money += 10;
    }

    public Attacker this[int index]
    {
        get
        {
            if (index < troops.Count)
                return troops[index];
            else
                return towers[index - troops.Count];
        }
    }

    internal void Clear()
    {
        graveyard.AddRange(troops);
        graveyard.AddRange(buildings);
        graveyard.AddRange(towers);

        troops.Clear();
        buildings.Clear();
        towers.Clear();
    }

    internal void ClearGraveyard()
    {
        graveyard.Clear();
    }

    public IEnumerator<Attacker> GetEnumerator()
    {
        foreach (TroopBase unit in troops)
            yield return unit;
        

        foreach (TowerBase tower in towers)
            yield return tower;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(IRecruitable recruit)
    {
        if (recruit is TroopBase troop)
            troops.Add(troop);
        else if (recruit is TowerBase tower)
            towers.Add(tower);
        else
            buildings.Add((Building)recruit);
    }

    public void Remove(IRecruitable recruit)
    {
        graveyard.Add(recruit);

        if (recruit is TroopBase troop)
            troops.Remove(troop);
        else if (recruit is TowerBase tower)
            towers.Remove(tower);
        else
            buildings.Remove((Building)recruit);
    }

    public TroopBase SenseEnemyLowestHealth() => enemyTroops.GetTrooOnCondition((x, y) => x.Health > y.Health);

    public TroopBase SenseEnemyHighestHealth() => enemyTroops.GetTrooOnCondition((x, y) => x.Health < y.Health);

    public TroopBase SenseEnemyLowestDamage() => enemyTroops.GetTrooOnCondition((x, y) => x.Damage > y.Damage);
           
    public TroopBase SeneseEnemyHighestDamage() => enemyTroops.GetTrooOnCondition((x, y) => x.Damage < y.Damage);
     
    public TroopBase SenseEnemyLowestSpeed() => enemyTroops.GetTrooOnCondition((x, y) => x.Speed > y.Speed);

    public TroopBase SenseEnemyHighestSpeed() => enemyTroops.GetTrooOnCondition((x, y) => x.Speed < y.Speed);

    public IAttack SenseClosestEnemy(IAttack attacker)
    {
        if (enemyTroops.Count == 0)
            return null;

        IAttack closest = enemyTroops[0];

        foreach (IAttack troop in enemyTroops)
        {
            if (Vector2Int.Distance(attacker.Position, closest.Position) > Vector2Int.Distance(attacker.Position, troop.Position))
                closest = troop;
        }

        return closest;
    }

    public TroopBase GetTroopFree() => troops.Find(x => x.CurrentState == State.Free);

    public TowerBase GetFreeTower() => towers.Find(x => x.CurrentState == State.Free);

    public TowerBase GetTowerUnderAttack() => towers.Find(x => x.CurrentState == State.UnderAttack);

    private TroopBase GetTrooOnCondition(SelectionPredicate condition)
    {
        if (troops.Count == 0)
            return null;

        TroopBase selected = troops[0];
        foreach (TroopBase troop in troops)
            if (condition(selected, troop))
                selected = troop;

        return selected;
    }

    public List<IRecruitable> GetDead()
    {
        return graveyard;
    }

    public List<Statistics> GetAllStats()
    {
        List<Statistics> stats = new List<Statistics>();

        foreach (TowerBase tower in towers)
            stats.Add(tower.GetStats());
        foreach (TroopBase troop in troops)
            stats.Add(troop.GetStats());
        foreach (Building building in buildings)
            stats.Add(building.GetStats());
        foreach (IRecruitable dead in graveyard)
            stats.Add(dead.GetStats());

        return stats;
    }
}
