using System;
using System.Collections;
using System.Collections.Generic;
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
        attacker.GiveDamage(enemy);
        
    }
}
