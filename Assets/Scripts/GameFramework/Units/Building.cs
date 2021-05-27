using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Damageable, IRecruitable
{ 
    private void Destroy()
    {
        CurrentInstance.GetArmy(Side).Remove(this);
        CurrentInstance.Map[Position] = null;

        if (Visual != null)
            UnityEngine.Object.Destroy(Visual);
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

    public Statistics GetStats() => new Statistics(0, ReceivedDamage, 0, 0, typeof(Building));
        
}

public class Wall : Building
{
    public override int Health { get; protected set; } = 30000;

    public override Vector2Int Position => position;

    public override Type type => typeof(Wall);

    private Vector2Int position;

    public Wall(Vector2Int pos, Instance instance, GameObject vis = null)
    {
        position = pos;
        Side = Role.Defender;
        CurrentInstance = instance;

        if (!CurrentInstance.IsTraining)
            Visual = vis;
    }
}


public class Gate : Wall
{
    public Gate(Vector2Int pos, Instance instance, GameObject vis = null):base(pos, instance, vis) { }

    public override bool CanPass(Role role)
    {
        if (role == Side)
            return true;

        return false;
    }
}


