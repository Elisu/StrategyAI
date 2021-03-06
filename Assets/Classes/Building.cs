using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDamageable
{
    public int Health { get; private set; }
    public int Defense { get; private set; }
    public bool Passable { get; private set; }

    public int Size { get; private set; }

    public Vector2Int Position { get; set; }

    public Role Side { get; private set; }

    public bool TakeDamage(int damage)
    {
        Health -= Defense * damage;

        if (Health <= 0)
            return true;

        return false;
    }
}
