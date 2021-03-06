using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    public int Health;
    public int Speed;
    public int Defense;
    public int Damage;
    public int Range;
    public int Size;
    public readonly bool Passable = false;

    [SerializeField]
    public Vector2Int Position { get; set; }

    public Role Side => throw new System.NotImplementedException();

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
