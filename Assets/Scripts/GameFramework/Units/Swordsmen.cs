using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Swordsmen : Human
{
    public Swordsmen()
    {
        Health = SwordsmenSetup.Health;
        Speed = SwordsmenSetup.Speed;
        Damage = SwordsmenSetup.Damage;
        Defense = SwordsmenSetup.Defense;
        Range = SwordsmenSetup.Range;
        UnitPrefab = SwordsmenSetup.UnitPrefab;
    }

    public override bool GiveDamage(IDamageable enemy, int totalDamage)
    {
        if (enemy is Building building)
            return building.TakeDamage(totalDamage);
        else
        {
            ITroop enemyTroop = (ITroop)enemy;
            int index = 0;

            while (totalDamage > 0)
            {
                if (enemyTroop.TakeDamage(Damage, index, 1))
                    return true;
                
                totalDamage -= Damage;
                index++;
            }

        }

        //IDamagable not killed
        return false;
    }
}
