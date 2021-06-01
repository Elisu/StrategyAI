using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : HumanUnit
{
    public Catapult()
    {
        Health = CatapultSetup.Health;
        Speed = CatapultSetup.Speed;
        Damage = CatapultSetup.Damage;
        Range = CatapultSetup.Range;
        UnitPrefab = CatapultSetup.UnitPrefab;
        BundleCount = 1;
    }

    internal override bool GiveDamage(Damageable enemy, int totalDamage)
    {
        if (enemy is TroopBase)
        {
            TroopBase enemyTroop = (TroopBase)enemy;

            while (totalDamage > 0)
            {
                int index = rnd.Next(0, enemyTroop.Count);
                int hitCount = rnd.Next(1, enemyTroop.Count - index);
                if (enemyTroop.TakeDamage(Damage, index, hitCount))
                    return true;

                totalDamage -= Damage;
            }
        }
        else
            return enemy.TakeDamage(totalDamage);

        //IDamagable not killed
        return false;
    }
}
