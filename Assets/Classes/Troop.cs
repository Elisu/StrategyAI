using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop<T>: ITroop where T: Unit, new()
{
    public Role Side { get; private set; }

    public int Count => troop.Count;

    public int Damage => Count * troop[0].Damage;

    public int Range => troop[0].Range;

    public int Defense { get; private set; }

    public int Health { get; private set; }

    public bool Passable => troop[0].Passable;

    public int Size => Count * troop[0].Size;

    public float Speed => troop[0].Speed;

    private Vector2 actualPosition;

    public Vector2 ActualPosition 
    {
        get { return actualPosition; }
        set
        {
            actualPosition = value;

            if (visual != null)
                visual.transform.position = new Vector3(actualPosition.x * MasterScript.map.SizeMultiplier,
                                                        1.5f,
                                                        actualPosition.y * MasterScript.map.SizeMultiplier);
        }        
    }

    public Vector2Int Position { get => Vector2Int.RoundToInt(ActualPosition); }

    public State CurrentState { get; private set; }

    private GameObject visual;

    private int MaxHealth;

    List<T> troop = new List<T>();
    System.Random rnd = new System.Random();

    public Troop(int count, Role side, GameObject prefab = null)
    {
        for (int i = 0; i < count; i++)
            troop.Add(new T());

        if (prefab != null)
            visual = prefab;

        Health = Count * troop[0].Health;
        MaxHealth = Health;
        Side = side;
        ActualPosition = MasterScript.map.GetFreeSpawn(Side);
        CurrentState = State.Free;

        MasterScript.map[Position] = this;
       
        if (visual != null)
            visual = Object.Instantiate(visual, visual.transform.position, Quaternion.identity);
    }


    public bool TakeDamage(int damage)
    {
        Health -= damage;
        while (damage > 0)
        {
            int randomUnit = rnd.Next(0, troop.Count - 1);
            Unit u = troop[randomUnit];

            if (u.TakeDamage(damage))
            {
                damage -= u.Health;
                troop.RemoveAt(randomUnit);
            }
        }
        return true;
    }

    public void TakeDamage(int damage, int index, int countToDamage)
    {
        Health -= damage;

        Debug.Log(string.Format("Current health {0}", Health));
        if (Health <= 0)
        {
            DestroyTroop();
            return;
        }

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
        CurrentState = State.Fighting;
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

    private void DestroyTroop()
    {
        MasterScript.GetArmy(Side).Remove(this);
        MasterScript.map[Position] = null;

        if (visual != null)
            Object.Destroy(visual);
    }

    public bool Move(Vector2Int nextPos)
    {
        if (MasterScript.map[nextPos] == null || MasterScript.map[nextPos].Passable == true)
        {
            MasterScript.map[Position] = null;
            Vector2 next = new Vector2(nextPos.x - Position.x, nextPos.y - Position.y);
            ActualPosition += next * Speed;
            MasterScript.map[Position] = this;

            if (Position == nextPos)
                return true;
        }
        else
        {
            RecomputeAndScheduleMove();
            return true;
        }

        return false;
    }

    private void RecomputeAndScheduleMove()
    {

    }
}
