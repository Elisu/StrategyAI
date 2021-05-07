using System.Collections.Generic;
using System.Collections;
using UnityEngine;

internal class Attack : IAction
{
    readonly Damageable target;
    readonly Attacker attacker;

    public Attack(Damageable defender, Attacker attacker)
    {
        target = defender;
        this.attacker = attacker;
    }

    public bool Execute()
    {
        if (attacker.Health <= 0 || target.Health <= 0)
        {
            attacker.StopAction();
            return false;
        }

        Vector2Int distance = attacker.Position - target.Position;
        //In case attack held on distant troop - have to move in range
        if ( Mathf.Abs(distance.x) > attacker.Range || Mathf.Abs(distance.y) > attacker.Range)
        {
            //Debug.Log(string.Format("MOVE Pos:{0}  TargetPos:{1}", attacker.Position, target.Position));
            if (attacker is TroopBase movableAttacker)
            {
                movableAttacker.MoveForAttack(target.Position);
                return true;

                //Always schedule again - needs to attack if move finished
                //return true;
            }
            else
                return false;
        }
        else
        {
            //Debug.Log(string.Format("Attacker health: {0}\nDefender health {1}", attacker.Health, target.Health));
            ((TroopBase)attacker).CorrectPosition();
            attacker.PrepareForAttack(target);
            bool stillGoing = attacker.Attack();

            if (!stillGoing)
                attacker.StopAction();

            return stillGoing;
        }

    }
    
}
