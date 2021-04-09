using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Human : Unit
{
    public int Damage { get; protected set; }
    public int Range { get; protected set; }

    public float Speed { get; protected set; }

    public virtual bool GiveDamage(IDamageable enemy, int totalDamage)
    {
       return enemy.TakeDamage(totalDamage);
    }

}
