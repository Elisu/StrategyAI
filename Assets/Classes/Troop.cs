using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop<T>: ITroop where T: Human, new()
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

    public State CurrentState { get; protected set; }

    public IDamageable Target { get; protected set; }

    public int DealtDamage { get; protected set; }

    public int ReceivedDamage { get; protected set; }

    private GameObject visual;

    private int MaxHealth;

    private Queue<Vector2Int> route;

    private Vector2Int targetPosition;

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

        if (Health <= 0)
        {
            DestroyTroop();
            return true;
        }

        while (damage > 0)
        {
            int randomUnit = rnd.Next(0, troop.Count - 1);
            Unit u = troop[randomUnit];

            bool killed = u.TakeDamage(damage);            
            damage -= u.Health;

            if (killed)
                troop.RemoveAt(randomUnit);
            
        }

        return false;
        
    }

    public bool TakeDamage(int damage, int index, int countToDamage)
    {
        Debug.Log(string.Format("Current health {0}", Health));        

        int countHit = 0;

        for (int i = index; i < Count; i++)
        {
            if (troop[i].TakeDamage(damage))
                troop.RemoveAt(i);

            countHit++;

            if (countHit == countToDamage)
                break;
        }

        Health -= damage * countHit;

        if (Health <= 0)
        {
            DestroyTroop();
            return true;
        }

        return false;
    }

    public bool GiveDamage(IDamageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;
        return troop[0].GiveDamage(enemy, Damage);
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

    private void RecomputeAndScheduleMove()
    {

    }

    public void StopAction()
    {
        CurrentState = State.Free;
        Target = null;
        route = null;
    }

    public void PrepareForAttack(IDamageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;
        route = null;        
    }

    public void PrepareForMove(Vector2Int targetPos)
    {
        CurrentState = State.Moving;
        Target = null;
        route = null;

        if (FindSpotInRange(targetPos, out Vector2Int inRange))
            RecomputeRoute(inRange);
    }

    public bool Move()
    {
        if (route == null)
            return false;

        Vector2Int nextPos = route.Peek();

        if (MasterScript.map[nextPos] == null || MasterScript.map[nextPos].Passable == true)
        {
            MasterScript.map[Position] = null;
            Vector2 next = new Vector2(nextPos.x - ActualPosition.x, nextPos.y - ActualPosition.y);
            ActualPosition += next * Speed;

            if (ActualPosition == nextPos)
                route.Dequeue();

            MasterScript.map[Position] = this;

            if (route.Count > 0)
                return true;
        }
        else
        {
            RecomputeRoute(targetPosition);

            if (route != null)
                return true;
        }

        StopAction();
        return false;
    }

    private void RecomputeRoute(Vector2Int targetPos)
    {
        List<Vector2Int> path = Pathfinding.FindPath(Position, targetPos);

        if (path == null)
        {
            route = null;
            return;
        }

        targetPosition = targetPos;
        route = new Queue<Vector2Int>(path);
    }

    private bool FindSpotInRange(Vector2Int target, out Vector2Int pos)
    {
        //float distance = float.MaxValue;

        for (int i = Mathf.Max(0, target.x - Range); i <= Mathf.Min(MasterScript.map.Width, target.x + Range); i++)
            for (int j = Mathf.Max(0, target.y - Range); j <= Mathf.Min(MasterScript.map.Height, target.y + Range); j++)
            {
                if (MasterScript.map[i,j].Passable)
                {
                    pos = new Vector2Int(i, j);
                    return true;
                }
            }

        pos = new Vector2Int(0, 0);
        return false;
    }

    public bool Attack()
    {
        if (Target == null)
            return false;

        if (GiveDamage(Target))
        {
            StopAction();
            return false;
        }
            

        return true;
    }
}
