using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit 
{
    public int Health { get; protected set; }

    public int Damage { get; protected set; }

    public int Defense { get; protected set; }

    public int Range { get; protected set; }

    public readonly bool Passable = false;

    internal virtual bool TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
            return true;

        return false;
    }

    internal virtual bool GiveDamage(Damageable enemy, int totalDamage)
    {
        return enemy.TakeDamage(totalDamage);
    }

}
