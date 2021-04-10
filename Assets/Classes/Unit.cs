using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    public int Health { get; protected set; }
    public int Defense { get; protected set; }
    public static int Size { get; protected set; }

    public readonly bool Passable = false;

    public virtual bool TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
            return true;

        return false;
    }

}
