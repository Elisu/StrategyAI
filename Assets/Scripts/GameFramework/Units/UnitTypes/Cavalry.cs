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
        Range = CavalrySetup.Range;
        UnitPrefab = CavalrySetup.UnitPrefab;
        BundleCount = 20;
    }
    public override float GetDefenseAgainst(Type unitType)
    {
        switch (unitType.Name)
        {
            case nameof(Archers):
                return 1;
            case nameof(Swordsmen):
                return 0.6f;
            default:
                return 0.5f;
        }
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
