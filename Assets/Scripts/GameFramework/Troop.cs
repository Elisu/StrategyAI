using System.Collections;
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
                if (visual != null)
                    visual.transform.position = new Vector3(actualPosition.x * CurrentInstance.Map.SizeMultiplier,
                                                            1.5f,
                                                            actualPosition.y * CurrentInstance.Map.SizeMultiplier);
        }
    }

    public override Vector2Int Position => Vector2Int.RoundToInt(ActualPosition);
    

    public abstract float Speed { get; protected set; }

    protected GameObject visual;

    protected int MaxHealth;

    protected Queue<Vector2Int> route;

    protected Vector2Int targetPosition;

    protected System.Random rnd = new System.Random();

    internal virtual void MoveForAttack(Vector2Int targetPos)
    {
        if (CurrentState != State.Moving || !CurrentInstance.Map[targetPosition].Passable || rnd.Next(0, 21) < 1)
        {
            CurrentState = State.Moving;
            Target = null;
            route = null;

            if (FindSpotInRange(targetPos, out Vector2Int inRange))
                RecomputeRoute(inRange);
        }

        Move();
        
        
    }

    private void RecomputeRoute(Vector2Int targetPos)
    {
        List<Vector2Int> path = Pathfinding.FindPath(Position, targetPos, CurrentInstance);

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
                if (CurrentInstance.Map[i, j].Passable)
                {
                    int diffX = Mathf.Abs(Position.x - i);
                    int diffY = Mathf.Abs(Position.y - j);
                    int min = Mathf.Min(diffX, diffY);

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


    public bool Move()
    {
        if (route == null)
            return false;

        Vector2Int nextPos = route.Peek();

        if (CurrentInstance.Map[nextPos] == null || CurrentInstance.Map[nextPos].Passable == true)
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

    public Statistics GetStats() => new Statistics(DealtDamage, ReceivedDamage, EnemiesKilled, BuildingsDestroyed);

}

public class Troop<T>: TroopBase where T: HumanUnit, new()
{
    protected List<T> troop = new List<T>();

    public override int Count => troop.Count;
    public override float Speed { get; protected set; }
    public override int Damage => troop[0].Damage * Count;
    public override int Range => troop[0].Range;
    public override int Defense => troop[0].Defense;
    public override int Health { get; protected set; }
    public override int Size => troop[0].Size * Count;

    public Troop(int count, Role side, Instance instance)
    {
        CurrentInstance = instance;

        for (int i = 0; i < count; i++)
            troop.Add(new T());

        if (!CurrentInstance.IsTraining)
            visual = HumanUnit.UnitPrefab;

        Health = Count * troop[0].Health;
        Speed = HumanUnit.Speed;
        MaxHealth = Health;
        Side = side;
        ActualPosition = CurrentInstance.Map.GetFreeSpawn(Side);
        CurrentState = State.Free;

        CurrentInstance.Map[Position] = this;

        if (CurrentInstance.IsTraining)
            return;

        if (visual != null)
        {
            visual = Object.Instantiate(visual, visual.transform.position, Quaternion.identity);
            CurrentInstance.GameOver += GameOver;

            if (Side == Role.Defender)
                visual.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }         
        
    }


    internal override bool TakeDamage(int damage)
    {
        Health -= damage;
        ReceivedDamage += damage;

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

    internal override bool TakeDamage(int damage, int index, int countToDamage)
    {
        //Debug.Log(string.Format("Current health {0}", Health));        

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
        DealtDamage += Damage;
        bool killed = troop[0].GiveDamage(enemy, Damage);

        if (killed)
        {
            if (enemy is TroopBase)
                EnemiesKilled++;

            if (enemy is Building || enemy is TowerBase)
                BuildingsDestroyed++;
        }

        return killed;
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
        CurrentInstance.GetArmy(Side).Remove(this);
        CurrentInstance.Map[Position] = null;

        if (visual != null)
            Object.Destroy(visual);
    }

    //public void StopAction()
    //{
    //    CurrentState = State.Free;
    //    Target = null;
    //    route = null;
    //    Action = null;
    //}

    //public void PrepareForAttack(IDamageable enemy)
    //{
    //    ActualPosition = Position;
    //    CurrentState = State.Fighting;
    //    Target = enemy;
    //    route = null;        
    //}

    //public void PrepareForMove(Vector2Int targetPos)
    //{
       
    //}   

    //public void SetAction(IAction action)
    //{
    //    CurrentState = State.PreparingForAction;
    //    route = null;
    //    Target = null;
    //    Action = action;
    //}

    private void GameOver()
    {
        DestroyTroop();
    }
}
