using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop<T>: ITroop where T: Unit, new()
{
    public Role Side { get; private set; }

    public int Count { get; private set; }
    public int Damage => Count * troop[0].damage;

    public int Range => troop[0].range;

    public int Defense { get; private set; }

    public int Health { get; private set; }

    public bool Passable => troop[0].Passable;

    public int Size => Count * troop[0].size;

    public float Speed => troop[0].speed;

    private Vector2 actualPosition;

    public Vector2 ActualPosition 
    {
        get { return actualPosition; }
        set
        {
            actualPosition = value;

            if (visual != null)
                visual.transform.position = actualPosition * MasterScript.map.SizeMultiplier;
        }        
    }

    public Vector2Int Position { get => Vector2Int.RoundToInt(ActualPosition); }

    public State CurrentState { get; private set; }

    private GameObject visual;

    List<T> troop = new List<T>();

    System.Random rnd;

    public Troop(int count, Role side, GameObject prefab = null)
    {
        for (int i = 0; i < count; i++)
            troop.Add(new T());

        ActualPosition = MasterScript.map.GetFreeSpawn(Side);
        Side = side;

        MasterScript.map[Position] = this;

        rnd = new System.Random();

        if (prefab != null)
        {
            visual = prefab;
            MonoBehaviour.Instantiate(visual, visual.transform.position, Quaternion.identity);
        }

        MasterScript.GetArmy(Side).Add(this);
    }


    public bool TakeDamage(int damage)
    {
        while (damage > 0)
        {
            int randomUnit = rnd.Next(0, troop.Count - 1);
            Unit u = troop[randomUnit];

            if (u.TakeDamage(damage))
            {
                damage -= u.health;
                troop.RemoveAt(randomUnit);
            }
        }
        return true;
    }

    public void TakeDamage(int damage, int index, int countToDamage)
    {
        for (int i = index; i < Count; i++)
        {
            if (troop[i].TakeDamage(damage))
                troop.RemoveAt(i);

            if (i + 1 == countToDamage)
                return;
        }
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
}
