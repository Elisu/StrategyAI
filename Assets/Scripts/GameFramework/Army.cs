using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : IEnumerable<IAttack>
{
    public Role Side { get; private set; }

    public int Count => army.Count + towers.Count;

    private List<ITroop> army = new List<ITroop>();
    private List<Building> buildings = new List<Building>();
    private List<Tower> towers = new List<Tower>();
    private Army enemyTroops;


    public Army(Role side)
    {
        Side = side;
    }

    public void SetEnemy()
    {
        enemyTroops = MasterScript.GetEnemyArmy(Side);
    }

    public IAttack this[int index]
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
        army.Clear();
        buildings.Clear();
        towers.Clear();
    }

    public IEnumerator<IAttack> GetEnumerator()
    {
        foreach (ITroop unit in army)
            yield return unit;
        

        foreach (Tower tower in towers)
            yield return tower;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(IRecruitable recruit)
    {
        if (recruit is ITroop troop)
            army.Add(troop);
        else if (recruit is Tower tower)
            towers.Add(tower);
        else
            buildings.Add((Building)recruit);
    }

    public void Remove(IRecruitable recruit)
    {
        if (recruit is ITroop troop)
            army.Remove(troop);
        else if (recruit is Tower tower)
            towers.Remove(tower);
        else
            buildings.Remove((Building)recruit);
    }

    public ITroop SenseEnemyLowestHealth() => enemyTroops.GetTrooOnCondition((x, y) => x.Health > y.Health);

    public ITroop SenseEnemyHighestHealth() => enemyTroops.GetTrooOnCondition((x, y) => x.Health < y.Health);

    public ITroop SenseEnemyLowestDamage() => enemyTroops.GetTrooOnCondition((x, y) => x.Damage > y.Damage);

    public ITroop SeneseEnemyHighestDamage() => enemyTroops.GetTrooOnCondition((x, y) => x.Damage < y.Damage);

    public ITroop SenseEnemyLowestDefense() => enemyTroops.GetTrooOnCondition((x, y) => x.Defense > y.Defense);

    public ITroop SenseEnemyHighestDefense() => enemyTroops.GetTrooOnCondition((x, y) => x.Defense < y.Defense);

    public ITroop SenseEnemyLowestSpeed() => enemyTroops.GetTrooOnCondition((x, y) => x.Speed > y.Speed);

    public ITroop SenseEnemyHighestSpeed() => enemyTroops.GetTrooOnCondition((x, y) => x.Speed < y.Speed);

    public ITroop GetTroopFree() => army.Find(x => x.CurrentState == State.Free);

    public Tower GetFreeTower() => towers.Find(x => x.CurrentState == State.Free);

    public Tower GetTowerUnderAttack() => towers.Find(x => x.CurrentState == State.UnderAttack);

    private ITroop GetTrooOnCondition(SelectionPredicate condition)
    {
        if (army.Count == 0)
            return null;

        ITroop selected = army[0];
        foreach (ITroop troop in army)
            if (condition(selected, troop))
                selected = troop;

        return selected;
    }
}
