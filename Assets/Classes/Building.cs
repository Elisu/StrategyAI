using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Unit, IRecruitable
{ 
    public State CurrentState { get; protected set; }

    public Role Side { get; protected set; }

    public Vector2Int Position { get; protected set; }

    bool IObject.Passable => Passable;

    int IObject.Size => Size;

    public int ReceivedDamage { get; protected set; }

    private GameObject visual;

    private void Destroy()
    {
        MasterScript.GetArmy(Side).Remove(this);
        MasterScript.map[Position] = null;

        if (visual != null)
            Object.Destroy(visual);
    }

    public override bool TakeDamage(int totalDamage)
    {
        ReceivedDamage += totalDamage * Defense;

        if (base.TakeDamage(totalDamage))
        {
            Destroy();
            return true;
        }

        return false;
    }
}
