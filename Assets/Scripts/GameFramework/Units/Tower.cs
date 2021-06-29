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

    public override int ReloadRate => unit.ReloadRate;

    public override Statistics GetStats() => new Statistics(DealtDamage, ReceivedDamage, EnemiesKilled, BuildingsDestroyed, type);

    protected T unit = new T();

    private Vector2Int position;

    private int reloadCountdown;

    internal override bool GiveDamage(Damageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;

        if (reloadCountdown > 0)
        {
            reloadCountdown--;
            return false;
        }

        int damage = Mathf.CeilToInt(Damage * GetDefenseAgainstMe(enemy));
        DealtDamage += damage;

        bool killed = enemy.TakeDamage(damage);

        if (killed)
            EnemiesKilled++;

        reloadCountdown = ReloadRate;
        return killed;
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
        CurrentInstance.Map[Position].OnField = null;

        if (Visual != null)
            GameObject.Destroy(Visual.gameObject);            
    }

    public override float GetDefenseAgainstMe(Damageable enemy)
    {
        return DamageModifiersMatrix.DamageModifier(enemy.type, type);
    }

    protected Tower() { }

    internal Tower(Vector2Int pos, Instance instance, VisualController vis = null)
    {
        CurrentState = State.Free;
        Side = Role.Defender;
        CurrentInstance = instance;
        Health = unit.Health;
        position = pos;
        reloadCountdown = unit.ReloadRate;

        if (!CurrentInstance.IsTraining)
        {
            Visual = vis;
            Visual.Set(this);
        }
            
    }
}
