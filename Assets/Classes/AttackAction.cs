using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Attack : IAction
{
    readonly IDamageable target;
    readonly IAttack attacker;

    public Attack(IDamageable defender, IAttack attacker)
    {
        target = defender;
        this.attacker = attacker;
    }

    public bool Execute()
    {
        if (attacker.Health <= 0)
            return false;

        //In case attack held on distant troop - have to move in range
        if (Vector2Int.Distance(attacker.Position, target.Position) > attacker.Range)
        {
            if (attacker is IMovable movableAttacker)
            {
                movableAttacker.PrepareForMove(target.Position);
                return (movableAttacker.Move());
            }
            else
                return false;
        }
        else
        {
            attacker.PrepareForAttack(target);
            return (attacker.Attack());
        }

    }
    
}
