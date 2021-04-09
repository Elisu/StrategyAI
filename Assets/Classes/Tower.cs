using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building, IAttack
{
    public int Damage { get; protected set; }
    public int Range { get; protected set; }

    public IDamageable Target { get; protected set; }

    public int DealtDamage { get; protected set; }

    protected Tower() { }

    public Tower(Role role)
    {
        Health = TowerSetup.Health;
        Damage = TowerSetup.Damage;
        Defense = TowerSetup.Defense;
        Range = TowerSetup.Range;
        CurrentState = State.Free;

        Side = role;
    }

    public virtual bool GiveDamage(IDamageable enemy)
    {
        CurrentState = State.Fighting;
        Target = enemy;
        DealtDamage += Damage * enemy.Defense;

        return enemy.TakeDamage(Damage);
    }

    public void StopAction()
    {
        throw new System.NotImplementedException();
    }

    public void PrepareForAttack(IDamageable enemy)
    {
        throw new System.NotImplementedException();
    }

    public bool Attack()
    {
        GiveDamage(Target);
        return true;
    }
}
