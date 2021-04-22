using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : Attacker, IRecruitable
{
    public Statistics GetStats() => new Statistics(DealtDamage, ReceivedDamage, EnemiesKilled, BuildingsDestroyed);
}

public class Tower<T> : TowerBase where T : TowerUnit
{
    public override int Damage { get;  }
    public override int Range { get; }
    public override int Defense { get; }
    public override int Health { get; protected set; }
    public override int Size { get; }
    public override Vector2Int Position { get; }

    //public int Damage => 
    //public int Range { get; protected set; }

    protected Tower() { }

    public Tower(Role role)
    {
        CurrentState = State.Free;
        Side = role;
    }
}
