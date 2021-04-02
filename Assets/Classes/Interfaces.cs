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
    Role Side { get;}

    bool Passable {get;}

    int Size { get; }
}

public interface IDamageable : IObject
{
    int Defense { get; }

    int Health { get; }

    bool TakeDamage(int damage);
}

public interface IMovable : IObject
{
    Vector2 ActualPosition { get; set; }

    float Speed { get; }

    public bool Move(Vector2Int targetPos);
}

public interface ITroop : IMovable, IAttack
{
    public int Count { get; }

    public void TakeDamage(int damage, int index, int count);
}

public interface IAttack : IDamageable
{
    int Damage { get; }

    int Range { get; }

    public State CurrentState { get; }

    public void GiveDamage(IDamageable enemy);
}

public interface IAction
{
    void Execute();
}

public interface IAI
{

}
