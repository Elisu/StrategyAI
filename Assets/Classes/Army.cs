using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : IEnumerable<ITroop>
{
    public Role Side { get; private set; }

    public int Count => army.Count;

    private List<ITroop> army = new List<ITroop>();

    public Army(Role side)
    {
        Side = side;
    }
    public ITroop this[int index]
    {
        get => army[index];
    }

    public IEnumerator<ITroop> GetEnumerator()
    {
        foreach (ITroop guest in army)
        {
            yield return guest;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(ITroop troop) => army.Add(troop);

    public void Remove(ITroop troop) => army.Remove(troop);

    public ITroop GetTroopFree() => army.Find(x => x.CurrentState == State.Free);

    public ITroop GetTroopLowestHealth() => GetTrooOnCondition((x, y) => x.Health > y.Health);

    public ITroop GetTroopHighestHealth() => GetTrooOnCondition((x, y) => x.Health < y.Health);

    public ITroop GetTroopLowestDamge() => GetTrooOnCondition((x, y) => x.Damage > y.Damage);

    public ITroop GetTroopHighestDamge() => GetTrooOnCondition((x, y) => x.Damage < y.Damage);

    public ITroop GetTroopLowestDefense() => GetTrooOnCondition((x, y) => x.Defense > y.Defense);

    public ITroop GetTroopHighestDefense() => GetTrooOnCondition((x, y) => x.Defense < y.Defense);

    public ITroop GetTroopLowestSpeed() => GetTrooOnCondition((x, y) => x.Speed > y.Speed);

    public ITroop GetTroopHighestSpeed() => GetTrooOnCondition((x, y) => x.Speed < y.Speed);

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
