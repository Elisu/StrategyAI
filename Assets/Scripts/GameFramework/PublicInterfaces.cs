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
}

public delegate bool SelectionPredicate(ITroop selected, ITroop current);

public interface IMappable
{
    Vector2Int Position { get; }
}

public interface IObject : IMappable
{
    bool Passable {get;}

    int Size { get; }
}

public interface IDamageable : IObject
{
    int Defense { get; }

    int Health { get; }

    Role Side { get; }

    int ReceivedDamage { get; }

    State CurrentState { get; }

    bool TakeDamage(int damage);  //
}

public abstract class Damageable : IDamageable
{
    public int Defense { get; protected set; }

    public int Health => throw new System.NotImplementedException();

    public Role Side => throw new System.NotImplementedException();

    public int ReceivedDamage => throw new System.NotImplementedException();

    public State CurrentState => throw new System.NotImplementedException();

    public bool Passable => throw new System.NotImplementedException();

    public int Size => throw new System.NotImplementedException();

    public Vector2Int Position => throw new System.NotImplementedException();

    public virtual bool TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}

public abstract class Attacker : Damageable, IAttack
{
    public int Damage => throw new System.NotImplementedException();

    public int Range => throw new System.NotImplementedException();

    public int DealtDamage => throw new System.NotImplementedException();

    public IAction Action => throw new System.NotImplementedException();

    public IDamageable Target => throw new System.NotImplementedException();

    public bool Attack()
    {
        throw new System.NotImplementedException();
    }

    public bool GiveDamage(IDamageable enemy)
    {
        throw new System.NotImplementedException();
    }

    public void PrepareForAttack(IDamageable enemy)
    {
        throw new System.NotImplementedException();
    }

    public void SetAction(IAction action)
    {
        throw new System.NotImplementedException();
    }

    public void StopAction()
    {
        throw new System.NotImplementedException();
    }
}


public interface IMovable : IObject
{
    Vector2 ActualPosition { get; }

    float Speed { get; }

    void PrepareForMove(Vector2Int targetPos); //

    bool Move(); //
}

public interface ITroop : IRecruitable, IAttack, IMovable
{
    int Count { get; }

    bool TakeDamage(int damage, int index, int count); //

    bool FindSpotInRange(Vector2Int target, out Vector2Int pos); //
}

public interface IAttack : IDamageable
{
    int Damage { get; }

    int Range { get; }

    int DealtDamage { get; }

    IAction Action { get; }

    IDamageable Target { get; }

    void SetAction(IAction action); //

    void StopAction(); //

    void PrepareForAttack(IDamageable enemy); //

    bool Attack(); //

    bool GiveDamage(IDamageable enemy); //

}

public interface IRecruitable : IDamageable
{

}

public interface IAction
{
    void Schedule();

    void Start();

    bool Execute();
}

public interface IAI
{

}

internal interface B
{
    void ahoj();
}

public abstract class Pozdrav : B
{

    public void ahoj()
    {
        throw new System.NotImplementedException();
    }

    internal void hello()
    {

    }
}

public class TR : Pozdrav
{

}
