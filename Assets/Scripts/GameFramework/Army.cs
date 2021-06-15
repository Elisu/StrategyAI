using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Army : IEnumerable<IRecruitable>
{
    public Role Side { get; private set; }

    public int Count => troops.Count + towers.Count + buildings.Count;

    public int Money { get; private set; }
    public int MoneySpent { get; private set; }
    public int MoneyAll => Money + MoneySpent;

    internal static int defenderStartMoney = 11000;
    internal static int defenderGain = 1000;
    internal static int attackerStartMoney = 15000;
    internal static int attackerGain = 1500;

    private readonly List<TroopBase> troops = new List<TroopBase>();
    private readonly List<Building> buildings = new List<Building>();
    private readonly List<TowerBase> towers = new List<TowerBase>();

    public ReadOnlyCollection<TroopBase> Troops => troops.AsReadOnly();
    public ReadOnlyCollection<TowerBase> Towers => towers.AsReadOnly();
    public ReadOnlyCollection<Building> Buildings => buildings.AsReadOnly();

    private List<IRecruitable> graveyard = new List<IRecruitable>();

    internal Army(Role side)
    {
        Side = side;
        if (Side == Role.Defender)
            Money += defenderStartMoney;
        else
            Money += attackerStartMoney;
    }

    internal IRecruitable AddStructure(string tag, Vector2Int position, Instance instance)
    {
        IRecruitable structure = UnitFactory.CreateStructure(tag, position, instance);
        Add(structure);
        return structure;
    }

    internal IRecruitable AddStructure(VisualController structureObject, Vector2Int position, Instance instance)
    {
        IRecruitable structure = UnitFactory.CreateStructure(structureObject, position, instance);
        Add(structure);
        return structure;
    }

    internal bool TryBuying(int toBuy, Instance instance)
    {
        if (toBuy < 0 || toBuy >= UnitFinder.UnitStats.Count)
            return false;

        Type unitType = UnitFinder.UnitStats[toBuy].UnitType;
        int price = UnitFinder.UnitStats[toBuy].Price;

        //Attacker can't buy towers
        if (Side == Role.Attacker && !(unitType.IsSubclassOf(typeof(HumanUnit))))
            return false;

        if (price <= Money && instance.Map.GetFreeSpawn(Side, out Vector2Int spawnPos))
        {
            Money -= price;
            MoneySpent += price;
            
            Add(UnitFactory.CreateUnit(unitType, Side, spawnPos, instance));
            return true;
        }

        return false;
    }

    internal void MoneyGain()
    {
        if (Side == Role.Defender)
            Money += defenderGain;
        else
            Money += attackerGain;
    }

    public IRecruitable this[int index]
    {
        get
        {
            if (index < troops.Count)
                return troops[index];
            else if (index - troops.Count < towers.Count)
                return towers[index - troops.Count];
            else
                return buildings[index - troops.Count - towers.Count];
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

    public IEnumerator<IRecruitable> GetEnumerator()
    {
        foreach (TroopBase unit in troops)
            yield return unit;        

        foreach (TowerBase tower in towers)
            yield return tower;

        foreach (Building building in buildings)
            yield return building;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Add(IRecruitable recruit)
    {
        if (recruit is TroopBase troop)
            troops.Add(troop);
        else if (recruit is TowerBase tower)
            towers.Add(tower);
        else
            buildings.Add((Building)recruit);
    }

    internal void Remove(IRecruitable recruit)
    {
        graveyard.Add(recruit);

        if (recruit is TroopBase troop)
            troops.Remove(troop);
        else if (recruit is TowerBase tower)
            towers.Remove(tower);
        else
            buildings.Remove((Building)recruit);
    }

    public IRecruitable SenseLowestHealth() => GetOnConditionFromAll((x, y) => x.Health > y.Health);

    public IRecruitable SenseHighestHealth() => GetOnConditionFromAll((x, y) => x.Health < y.Health);

    public TroopBase SenseTroopLowestDamage() => GetOnConditionOfType((x, y) => x.Damage > y.Damage, troops);
           
    public TroopBase SeneseTroopHighestDamage() => GetOnConditionOfType((x, y) => x.Damage < y.Damage, troops);
     
    public TroopBase SenseTroopLowestSpeed() => GetOnConditionOfType((x, y) => x.Speed > y.Speed, troops);

    public TroopBase SenseTroopHighestSpeed() => GetOnConditionOfType((x, y) => x.Speed < y.Speed, troops);

    public IRecruitable SenseLowestDamageDecrease(Attacker attacker)
    {
        if (troops.Count + towers.Count == 0)
            return null;

        Damageable withLowest = troops[0];
        float defense = attacker.GetDefenseAgainstMe(withLowest);

        foreach (Attacker troop in troops)
        {
            if (attacker.GetDefenseAgainstMe(troop) > defense)
                withLowest = troop;
        }

        return (IRecruitable)withLowest;
    }

    public IRecruitable SenseClosestTo(Attacker attacker)
    {
        if (Count == 0)
            return null;

        IRecruitable closest = this[0];

        foreach (IRecruitable troop in this)
        {
            if (Vector2Int.Distance(attacker.Position, closest.Position) > Vector2Int.Distance(attacker.Position, troop.Position))
                closest = troop;
        }

        return closest;
    }

    public TroopBase GetTroopFree() => troops.Find(x => x.CurrentState == State.Free);

    public TowerBase GetFreeTower() => towers.Find(x => x.CurrentState == State.Free);

    public TowerBase GetTowerUnderAttack() => towers.Find(x => x.CurrentState == State.UnderAttack);

    private UnitType GetOnConditionOfType<UnitType>(SelectionPredicate<UnitType> condition, IList<UnitType> source) where UnitType : class, IRecruitable
    {
        if (source.Count == 0)
            return null;

        UnitType selected = source[0];
        foreach (UnitType troop in source)
            if (condition(selected, troop))
                selected = troop;

        return selected;
    }

    private IRecruitable GetOnConditionFromAll(SelectionPredicate<IRecruitable> condition)
    {
        if (Count == 0)
            return null;

        IRecruitable selected = this[0];
        foreach (IRecruitable troop in this)
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
