using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Human : Unit
{
    public static int Damage { get; protected set; }
    public static int Range { get; protected set; }

    public static float Speed { get; protected set; }

    public static GameObject UnitPrefab { get; protected set; }

    public virtual bool GiveDamage(IDamageable enemy, int totalDamage)
    {
       return enemy.TakeDamage(totalDamage);
    }

}
