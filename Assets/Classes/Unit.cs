using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    public int Health { get; protected set; }
    public int Speed { get; protected set; }
    public int Defense { get; protected set; }
    public int Damage { get; protected set; }
    public int Range { get; protected set; }
    public int Size { get; protected set; }

    public readonly bool Passable = false;

    public bool TakeDamage(int damage)
    {
        Health -= Defense * damage;

        if (Health <= 0)
            return true;

        return false;
    }

    public virtual void GiveDamage(IDamageable enemy, int totalDamage)
    {
        enemy.TakeDamage(totalDamage);
    }

}
