using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Swordsmen : Unit
{
    public Swordsmen()
    {
        Health = SwordsmenSetup.Health;
        Speed = SwordsmenSetup.Speed;
        Damage = SwordsmenSetup.Damage;
        Defense = SwordsmenSetup.Defense;
        Range = SwordsmenSetup.Range;
    }

    public override void GiveDamage(IDamageable enemy, int totalDamage)
    {
        if (enemy is Building building)
            building.TakeDamage(totalDamage);
        else
        {
            ITroop enemyTroop = (ITroop)enemy;
            int index = 0;

            while (totalDamage > 0)
            {
                enemyTroop.TakeDamage(Damage, index, 1);
                totalDamage -= Damage;
                index++;
            }

        }        
    }
}
