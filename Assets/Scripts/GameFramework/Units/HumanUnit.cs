using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanUnit : Unit
{
    public int Size { get; protected set;}

    public float Speed { get; protected set; }

    public int Health { get; protected set; }

    public int Damage { get; protected set; }

    public int Range { get; protected set; }

    public int Price { get; protected set; }

    public int BundleCount { get; protected set; }

    public bool Passable => false;

    public GameObject UnitPrefab { get; protected set; }

    protected System.Random rnd = new System.Random();

    internal virtual bool GiveDamage(Damageable enemy, int totalDamage)
    {
        return enemy.TakeDamage(totalDamage);
    }

    public abstract float GetDefenseAgainst(Type unitType);
}
