using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    public int health;
    public int speed;
    public int defense;
    public int damage;
    public int range;
    public int size;
    public readonly bool Passable = false;

    public bool TakeDamage(int damage)
    {
        health -= defense * damage;

        if (health <= 0)
            return true;

        return false;
    }

    public virtual void GiveDamage(IDamageable enemy, int totalDamage)
    {
        enemy.TakeDamage(totalDamage);
    }

}
