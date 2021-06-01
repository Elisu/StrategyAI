using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Swordsmen : HumanUnit
{
    public Swordsmen()
    {
        Health = SwordsmenSetup.Health;
        Speed = SwordsmenSetup.Speed;
        Damage = SwordsmenSetup.Damage;
        Range = SwordsmenSetup.Range;
        UnitPrefab = SwordsmenSetup.UnitPrefab;
        BundleCount = 50;
    }

    internal override bool GiveDamage(Damageable enemy, int totalDamage)
    {
        if (enemy is TroopBase)
        {
            TroopBase enemyTroop = (TroopBase)enemy;
            int index = 0;

            while (totalDamage > 0)
            {
                if (enemyTroop.TakeDamage(Damage, index, 1))
                    return true;

                totalDamage -= Damage;
                index++;
            }
        }            
        else
            return enemy.TakeDamage(totalDamage);

        //IDamagable not killed
        return false;
    }
}
