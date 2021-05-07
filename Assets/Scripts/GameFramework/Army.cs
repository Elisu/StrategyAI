using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : IEnumerable<Attacker>
{
    public Role Side { get; private set; }

    public int Count => army.Count + towers.Count;

    public Statistics Stats { get; private set; }

    private List<TroopBase> army = new List<TroopBase>();
    private List<Building> buildings = new List<Building>();
    private List<TowerBase> towers = new List<TowerBase>();
    private Army enemyTroops;

    private List<IRecruitable> graveyard = new List<IRecruitable>();


    public Army(Role side)
    {
        Side = side;
    }

    public void SetEnemy(Instance instance)
    {
        enemyTroops = instance.GetEnemyArmy(Side);
    }

    public Attacker this[int index]
    {
        get
        {
            if (index < army.Count)
                return army[index];
            else
                return towers[index - army.Count];
        }
    }

    internal void Clear()
    {
        graveyard.AddRange(army);
        graveyard.AddRange(buildings);
        graveyard.AddRange(towers);

        army.Clear();
        buildings.Clear();
        towers.Clear();
    }

    internal void ClearGraveyard()
    {
        graveyard.Clear();
    }

    public IEnumerator<Attacker> GetEnumerator()
    {
        foreach (TroopBase unit in army)
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
            army.Add(troop);
        else if (recruit is TowerBase tower)
            towers.Add(tower);
        else
            buildings.Add((Building)recruit);
    }

    public void Remove(IRecruitable recruit)
    {
        graveyard.Add(recruit);

        if (recruit is TroopBase troop)
            army.Remove(troop);
        else if (recruit is TowerBase tower)
            towers.Remove(tower);
        else
            buildings.Remove((Building)recruit);
    }

    public TroopBase SenseEnemyLowestHealth() => enemyTroops.GetTrooOnCondition((x, y) => x.Health > y.Health);

    public TroopBase SenseEnemyHighestHealth() => enemyTroops.GetTrooOnCondition((x, y) => x.Health < y.Health);

    public TroopBase SenseEnemyLowestDamage() => enemyTroops.GetTrooOnCondition((x, y) => x.Damage > y.Damage);
           
    public TroopBase SeneseEnemyHighestDamage() => enemyTroops.GetTrooOnCondition((x, y) => x.Damage < y.Damage);
        
    public TroopBase SenseEnemyLowestDefense() => enemyTroops.GetTrooOnCondition((x, y) => x.Defense > y.Defense);
        
    public TroopBase SenseEnemyHighestDefense() => enemyTroops.GetTrooOnCondition((x, y) => x.Defense < y.Defense);
     
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

    public TroopBase GetTroopFree() => army.Find(x => x.CurrentState == State.Free);

    public TowerBase GetFreeTower() => towers.Find(x => x.CurrentState == State.Free);

    public TowerBase GetTowerUnderAttack() => towers.Find(x => x.CurrentState == State.UnderAttack);

    private TroopBase GetTrooOnCondition(SelectionPredicate condition)
    {
        if (army.Count == 0)
            return null;

        TroopBase selected = army[0];
        foreach (TroopBase troop in army)
            if (condition(selected, troop))
                selected = troop;

        return selected;
    }

    public List<IRecruitable> GetDead()
    {
        return graveyard;
    }
}
