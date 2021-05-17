using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : Attacker, IRecruitable
{
    public abstract Statistics GetStats();
}

public class Tower<T> : TowerBase where T : TowerUnit, new()
{
    public override int Damage { get;  }
    public override int Range { get; }
    public override int Health { get; protected set; }
    public override int Size { get; }
    public override Vector2Int Position { get; }

    public override Type type => typeof(T);

    public override Statistics GetStats() => new Statistics(DealtDamage, ReceivedDamage, EnemiesKilled, BuildingsDestroyed, type);

    protected T unit = new T();

    internal override bool GiveDamage(Damageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;

        int damage = Mathf.CeilToInt(Damage * unit.GetDefenseAgainst(enemy.type));
        DealtDamage += damage;

        return enemy.TakeDamage(damage);
    }

    protected Tower() { }

    public Tower(Role role)
    {
        CurrentState = State.Free;
        Side = role;
    }
}
