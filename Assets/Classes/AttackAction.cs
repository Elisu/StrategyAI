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
        attacker.PrepareForAction();
    }

    public bool Execute()
    {
        if (attacker.Health <= 0 || target.Health <= 0)
        {
            attacker.StopAction();
            return false;
        }

        //In case attack held on distant troop - have to move in range
        if (Vector2Int.Distance(attacker.Position, target.Position) > attacker.Range)
        {
            if (attacker is IMovable movableAttacker)
            {
                movableAttacker.PrepareForMove(target.Position);
                movableAttacker.Move();

                //Always schedule again - needs to attack if move finished
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log(string.Format("Attacker health: {0}\nDefender health {1}", attacker.Health, target.Health));
            attacker.PrepareForAttack(target);
            bool stillGoing = attacker.Attack();

            if (!stillGoing)
                attacker.StopAction();

            return stillGoing;
        }

    }
    
}
