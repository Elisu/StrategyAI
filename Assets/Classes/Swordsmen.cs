using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsmen : Unit
{
    public override void GiveDamage(IDamageable enemy, int totalDamage)
    {
        if ((Building)enemy != null)
            enemy.TakeDamage(totalDamage);
        else
        {
            ITroop enemyTroop = (ITroop)enemy;
            int index = 0;

            while (totalDamage > 0)
            {
                enemyTroop.TakeDamage(damage, index, 1);
                totalDamage -= damage;
                index++;
            }

        }        
    }
}
