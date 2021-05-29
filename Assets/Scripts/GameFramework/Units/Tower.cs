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
    public override int Damage => unit.Damage;
    public override int Range => unit.Range;
    public override int Health { get; protected set; }
    public override Vector2Int Position => position;

    public override Type type => typeof(T);

    public override Statistics GetStats() => new Statistics(DealtDamage, ReceivedDamage, EnemiesKilled, BuildingsDestroyed, type);

    protected T unit = new T();

    private Vector2Int position;

    internal override bool GiveDamage(Damageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;

        int damage = Mathf.CeilToInt(Damage * unit.GetDefenseAgainst(enemy.type));
        DealtDamage += damage;

        return enemy.TakeDamage(damage);
    }

    internal override bool TakeDamage(int totalDamage)
    {
        ReceivedDamage += totalDamage;

        if (base.TakeDamage(totalDamage))
        {
            Destroy();
            return true;
        }

        return false;
    }

    private void Destroy()
    {
        CurrentInstance.GetArmy(Side).Remove(this);
        CurrentInstance.Map[Position] = null;

        if (Visual != null)
            GameObject.Destroy(Visual.gameObject);            
    }

    protected Tower() { }

    internal Tower(Vector2Int pos, Instance instance, VisualController vis = null)
    {
        CurrentState = State.Free;
        Side = Role.Defender;
        CurrentInstance = instance;
        Health = unit.Health;
        position = pos;

        if (!CurrentInstance.IsTraining)
        {
            Visual = vis;
            Visual.Set(this);
        }
            
    }
}
