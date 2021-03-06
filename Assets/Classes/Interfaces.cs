using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    attacker,
    defender,
}

public interface IObject
{
    Vector2Int Position { get;}

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
}

public interface ITroop : IMovable, IAttack
{
    public int Count { get; }
}

public interface IAttack : IDamageable
{
    int Damage { get; }

    int Range { get; }
}

public interface IAction
{
    void Execute();
}
