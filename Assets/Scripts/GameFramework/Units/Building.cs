using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Damageable, IRecruitable
{ 
    private GameObject visual;

    private void Destroy()
    {
        CurrentInstance.GetArmy(Side).Remove(this);
        CurrentInstance.Map[Position] = null;

        if (visual != null)
            Object.Destroy(visual);
    }

    internal override bool TakeDamage(int totalDamage)
    {
        ReceivedDamage += totalDamage * Defense;

        if (base.TakeDamage(totalDamage))
        {
            Destroy();
            return true;
        }

        return false;
    }

    public Statistics GetStats() => new Statistics(0, ReceivedDamage, 0, 0, typeof(Building));
        
}
