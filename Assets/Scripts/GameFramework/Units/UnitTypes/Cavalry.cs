using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavalry : HumanUnit
{
    public Cavalry()
    {
        Health = CavalrySetup.Health;
        Speed = CavalrySetup.Speed;
        Damage = CavalrySetup.Damage;
        ReloadRate = CavalrySetup.ReloadRate;
        Range = CavalrySetup.Range;
        UnitPrefab = CavalrySetup.UnitPrefab;
        BundleCount = CavalrySetup.BundleCount;
    }

    internal override bool GiveDamage(Damageable enemy, int totalDamage)
    {
        if (enemy is TroopBase enemyTroop)
        {
            int index = rnd.Next(0, enemyTroop.Count);

            while (totalDamage > 0)
            {
                if (enemyTroop.TakeDamage(Damage, index, 5))
                    return true;

                totalDamage -= Damage;
                index = rnd.Next(0, enemyTroop.Count);
            }
        }
        else
            enemy.TakeDamage(totalDamage);



        //IDamagable not killed
        return false;
    }
}
