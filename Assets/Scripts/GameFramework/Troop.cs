using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class TroopBase : Attacker, IMovable, IRecruitable
{
    public abstract int Count { get;}

    private Vector2 actualPosition;

    public Vector2 ActualPosition
    {
        get { return actualPosition; }
        set
        {
            actualPosition = value;

            if (!CurrentInstance.IsTraining)
                if (Visual != null)
                    Visual.transform.position = new Vector3(actualPosition.x * mapMultiplier,
                                                            Visual.transform.position.y,
                                                            actualPosition.y * mapMultiplier);
        }
    }

    public override Vector2Int Position => Vector2Int.RoundToInt(ActualPosition);
    

    public abstract float Speed { get; }

    protected int MaxHealth;

    protected Queue<Vector2Int> route;

    protected Vector2Int targetPosition;

    protected System.Random rnd = new System.Random();

    protected float mapMultiplier;

    internal void MoveForAttack(Vector2Int targetPos)
    {
        if (CurrentState != State.Moving || !CurrentInstance.Map[targetPosition].CanPass(Side) || rnd.Next(0, 201) < 1)
        {
            CurrentState = State.Moving;
            Target = null;
            route = null;

            if (FindSpotInRange(targetPos, out Vector2Int inRange))
                RecomputeRoute(inRange);
        }

        Move();        
    }

    internal bool MoveTo(Vector2Int targetPos)
    {
        if (CurrentState != State.Moving || !CurrentInstance.Map[targetPosition].CanPass(Side) || rnd.Next(0, 201) < 1)
        {
            CurrentState = State.Moving;
            Target = null;
            route = null;
            RecomputeRoute(targetPos);
        }

        return Move();
    }

    private void RecomputeRoute(Vector2Int targetPos)
    {
        List<Vector2Int> path = Pathfinding.FindPath(Position, targetPos, Side, CurrentInstance);

        if (path == null)
        {
            route = null;
            CurrentState = State.Free;
            return;
        }

        targetPosition = targetPos;
        route = new Queue<Vector2Int>(path);
    }

    internal bool FindSpotInRange(Vector2Int target, out Vector2Int pos)
    {
        int distance = int.MaxValue;
        Vector2Int coords = new Vector2Int(0, 0);
        bool found = false;

        for (int i = Mathf.Max(0, target.x - this.Range); i < Mathf.Min(CurrentInstance.Map.Width, target.x + this.Range + 1); i++)
            for (int j = Mathf.Max(0, target.y - this.Range); j < Mathf.Min(CurrentInstance.Map.Height, target.y + this.Range + 1); j++)
            {
                if (CurrentInstance.Map[i, j].CanPass(Side))
                {
                    int diffX = Mathf.Abs(Position.x - i);
                    int diffY = Mathf.Abs(Position.y - j);
                    int min = diffX + diffY;

                    if (distance > min)
                    {
                        distance = min;
                        coords = new Vector2Int(i, j);
                        found = true;
                    }
                }
            }

        pos = coords;
        return found;
    }

    private bool Move()
    {
        if (route == null || route.Count == 0)
            return false;

        Vector2Int nextPos = route.Peek();

        if (CurrentInstance.Map[nextPos] == null || CurrentInstance.Map[nextPos].CanPass(Side) == true || nextPos == Position)
        {
            CurrentInstance.Map[Position] = null;
            Vector2 next = new Vector2(nextPos.x - ActualPosition.x, nextPos.y - ActualPosition.y);

            if (Mathf.Abs(next.normalized.x * Speed) > Mathf.Abs(next.x) || Mathf.Abs(next.normalized.y * Speed) > Mathf.Abs(next.y))
                ActualPosition = nextPos;
            else
                ActualPosition += next.normalized * Speed;

            if (ActualPosition == nextPos)
            {
                route.Dequeue();
            }

            CurrentInstance.Map[Position] = this;

            if (route.Count > 0)
                return true;
        }
        else
        {
            RecomputeRoute(targetPosition);

            int k;
            if (route != null)
                if (route.Count == 0)
                    k = 5;

            if (route != null)
                return true;
        }



        return false;
    }

    internal abstract bool TakeDamage(int damage, int index, int countToDamage);

    internal void CorrectPosition()
    {
        ActualPosition = Position;
    }

    public abstract Statistics GetStats();
}

public class Troop<T>: TroopBase where T: HumanUnit, new()
{
    private List<int> troopHealth = new List<int>();
    private T unit = new T();

    private static ConcurrentDictionary<Type, float> defenseCache = new ConcurrentDictionary<Type, float>();

    public override int Count => troopHealth.Count;
    public override float Speed => unit.Speed;
    public override int Damage => unit.Damage * Count;
    public override int Range => unit.Range;
    public override int Health { get; protected set; }

    public override Type type => typeof(T);

    public override Statistics GetStats() => new Statistics(DealtDamage, ReceivedDamage, EnemiesKilled, BuildingsDestroyed, typeof(T));

    public Troop(Role side, Vector2Int spawnPos,  Instance instance)
    {
        CurrentInstance = instance;
        mapMultiplier = instance.Map.SizeMultiplier;

        for (int i = 0; i < unit.BundleCount; i++)
            troopHealth.Add(unit.Health);

        if (!CurrentInstance.IsTraining)
            Visual = unit.UnitPrefab;

        Health = Count * unit.Health;
        MaxHealth = Health;
        Side = side;
        ActualPosition = spawnPos;
        CurrentState = State.Free;

        CurrentInstance.Map[Position] = this;

        if (CurrentInstance.IsTraining)
            return;

        if (Visual != null)
        {
            Visual = UnityEngine.Object.Instantiate(Visual, Visual.transform.position, Quaternion.identity);
            Visual.Set(this);
        }        
    }


    internal override bool TakeDamage(int damage)
    {
        ReceivedDamage += damage;

        if (Health - damage <= 0)
        {
            Health = 0;
            DestroyTroop();
            return true;
        }

        while (damage > 0 && Count > 0)
        {
            int randomUnit = rnd.Next(0, Count - 1);

            int unitHealth = troopHealth[randomUnit];
            troopHealth[randomUnit] -= damage;
            Health -= Mathf.Min(unitHealth, damage);
            damage -= unitHealth;

            if (troopHealth[randomUnit] < 0)
                troopHealth.RemoveAt(randomUnit);            
        }

        return false;
        
    }

    internal override bool TakeDamage(int damage, int index, int countToDamage)
    {
        //Debug.Log(string.Format("Current health {0}", Health));        

        int countHit = 0;

        for (int i = index; i < Count; i++)
        {
            int unitHealth = troopHealth[i];
            troopHealth[i] -= damage;
            Health -= Mathf.Min(unitHealth, damage);

            if (troopHealth[i] <= 0)
                troopHealth.RemoveAt(i);              

            countHit++;

            if (countHit == countToDamage)
                break;
        }

        ReceivedDamage += damage * countHit;

        if (Health <= 0)
        {
            DestroyTroop();
            return true;
        }

        return false;
    }

    internal override bool GiveDamage(Damageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;

        float defense;

        if (!defenseCache.TryGetValue(enemy.type, out defense))
        {
            defense = GetDefenseAgainstMe(enemy);
            defenseCache.TryAdd(enemy.type, defense);
        }

        int damage = Mathf.CeilToInt(Damage * defense);
        DealtDamage += damage;
        bool killed = unit.GiveDamage(enemy, damage);

        if (killed)
        {
            if (enemy is TroopBase)
                EnemiesKilled++;

            if (enemy is Building || enemy is TowerBase)
                BuildingsDestroyed++;
        }

        return killed;
    }

    private void DestroyTroop()
    {
        CurrentInstance.GetArmy(Side).Remove(this);
        CurrentInstance.Map[Position] = null;

        if (Visual != null)
            UnityEngine.Object.Destroy(Visual.gameObject);
    }

    public override float GetDefenseAgainstMe(Damageable enemy)
    {
        return DefenseModifiersMatrix.GetDefense(enemy.type, type);
    }
}
