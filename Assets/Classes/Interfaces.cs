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

    bool TakeDamage(int damage);
}

public interface IMovable : IObject
{
    Vector2 ActualPosition { get; set; }

    float Speed { get; }

    void PrepareForMove(Vector2Int targetPos);

    bool Move();
}

public interface ITroop : IRecruitable, IAttack, IMovable
{
    int Count { get; }

    bool TakeDamage(int damage, int index, int count);
}

public interface IAttack : IDamageable
{
    int Damage { get; }

    int Range { get; }

    int DealtDamage { get; }

    IDamageable Target { get; }

    void PrepareForAction();

    void StopAction();

    void PrepareForAttack(IDamageable enemy);

    bool Attack();

    bool GiveDamage(IDamageable enemy);

}

public interface IRecruitable : IDamageable
{
    State CurrentState { get; }

    int ReceivedDamage { get; }
}

public interface IAction
{
    bool Execute();
}

public interface IAI
{

}
