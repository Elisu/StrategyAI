using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop<T>: MonoBehaviour, ITroop where T: Unit, new()
{
    public Role Side { get; private set; }

    public int Count { get; private set; }
    public int Damage => Count * troop[0].Damage;

    public int Range => troop[0].Range;

    public int Defense { get; private set; }

    public int Health { get; private set; }

    public bool Passable => troop[0].Passable;

    public int Size => Count * troop[0].Size;

    public float Speed => troop[0].Speed;

    public Vector2 ActualPosition { get; set; }

    public Vector2Int Position { get => Vector2Int.RoundToInt(ActualPosition); }

    List<T> troop = new List<T>();
    System.Random rnd;
    IDamageable target;

    public Troop(int count, Role side)
    {
        for (int i = 0; i < count; i++)
            troop.Add(new T());

        Side = side;
        rnd = new System.Random();
    }


    public bool TakeDamage(int damage)
    {
        //troop.GiveDamage(troop);

        while (damage > 0)
        {
            int randomUnit = rnd.Next(0, troop.Count - 1);
            Unit u = troop[randomUnit];

            if (u.TakeDamage(damage) == true)
            {
                damage -= u.Health;
                troop.RemoveAt(randomUnit);
            }
        }
        return true;
    }

    public void GiveDamage(IDamageable enemy)
    {
        troop[0].GiveDamage(enemy, Damage);
    }

    public Troop<T> Split(int count)
    {
        return null;
    }

    public void AddUnit(T u)
    {
        troop.Add(u);
    }

    public void UniteTroops(Troop<T> other)
    {
        foreach (T u in other.troop)
            troop.Add(u);
    }

    private void KillUnit(T u)
    {
        troop.Remove(u);
    }
}
