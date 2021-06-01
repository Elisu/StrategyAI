using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    Attacker,
    Defender,
    Neutral,
}

public enum State
{
    Moving,
    Fighting,
    UnderAttack,
    PreparingForAction,
    Free,
}

public enum SquareType
{
    Grass,
    Water,
    Debris,
    Spawn, 
    Building,
}

public delegate bool SelectionPredicate<UnitType>(UnitType selected, UnitType current) where UnitType : IRecruitable;

public interface IMappable
{
    Vector2Int Position { get; }
}

public interface IObject : IMappable
{
    bool CanPass(Role role);
}

public interface IDamageable : IObject
{
    Type type { get; }

    int Health { get; }

    Role Side { get; }

    int ReceivedDamage { get; }

    State CurrentState { get; }

}

public abstract class Damageable : IDamageable
{
    public abstract int Health { get; protected set; }

    public Role Side { get; protected set; }

    public int ReceivedDamage { get; protected set; }

    public State CurrentState { get; protected set; }

    internal VisualController Visual { get; private protected set; }

    public abstract Vector2Int Position { get; }
    public abstract Type type { get; }
    internal Instance CurrentInstance { get; private protected set; }

    public virtual bool CanPass(Role role) => false;

    internal virtual bool TakeDamage(int damage)
    {
        Health -= damage;
        ReceivedDamage += damage;

        if (Health <= 0)
            return true;

        return false;
    }
}

public abstract class Attacker : Damageable, IAttack
{
    public abstract int Damage { get; }

    public abstract int Range { get; }

    public int DealtDamage { get; protected set; }

    public Damageable Target { get; protected set; }

    public int EnemiesKilled { get; protected set; }

    public int BuildingsDestroyed { get; protected set; }

    internal IAction Action { get; private protected set; }

    /// <summary>
    /// Function which attacks target set in object
    /// </summary>
    /// <returns>
    ///     True -> continue with attack  
    ///     False -> end attack
    /// </returns>
    internal virtual bool Attack()
    {
        //No target - do not want to reschedule action
        if (Target == null)
            return false;

        //If true - target killed --> do not reschedule
        if (GiveDamage(Target))
            return false;        

        return true;
    }

    internal abstract bool GiveDamage(Damageable enemy);

    internal virtual void PrepareForAttack(Damageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;
    }

    internal virtual void SetAction(IAction action)
    {
        CurrentState = State.PreparingForAction;
        Target = null;
        Action = action;
    }

    internal virtual void StopAction()
    {
        CurrentState = State.Free;
        Target = null;
        Action = null;
    }

    public abstract float GetDefenseAgainstMe(Damageable enemy);
}


public interface IMovable : IObject
{
    Vector2 ActualPosition { get; }

    float Speed { get; }
}


public interface IAttack : IDamageable
{
    int Damage { get; }

    int Range { get; }

    int DealtDamage { get; }

    int EnemiesKilled { get; }

    int BuildingsDestroyed { get; }

    Damageable Target { get; }

}

public interface IRecruitable : IDamageable
{
    public Statistics GetStats();
}

public interface IAction
{
    bool Execute();
}

public interface IAI
{

}

