using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Attack : IAction
{
    Vector2Int targetPosition;
    IAttack attacker;

    public Attack(int x, int y, IAttack attacker)
    {
        targetPosition.x = x;
        targetPosition.y = y;
        this.attacker = attacker;
    }

    public Attack(IDamageable defender, IAttack attacker)
    {
        targetPosition = defender.Position;
        this.attacker = attacker;
    }

    public void Execute()
    {
        IDamageable enemy = (IDamageable)MasterScript.map[targetPosition.x, targetPosition.y];

        //In case attack held on distant troop - have to move in range
        if (Vector2Int.Distance(attacker.Position, targetPosition) > attacker.Range)
        {

        }
        
    }
    
}
