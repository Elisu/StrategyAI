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
        Defense = CavalrySetup.Defense;
        Range = CavalrySetup.Range;
        UnitPrefab = CavalrySetup.UnitPrefab;
    }

    internal override bool GiveDamage(Damageable enemy, int totalDamage)
    {
        if (enemy is Building building)
            return building.TakeDamage(totalDamage);
        else
        {
            TroopBase enemyTroop = (TroopBase)enemy;
            int index = Random.Range(0, enemyTroop.Count);

            while (totalDamage > 0)
            {
                if (enemyTroop.TakeDamage(Damage, index , 5))
                    return true;

                totalDamage -= Damage;
                index = Random.Range(0, enemyTroop.Count);
            }

        }

        //IDamagable not killed
        return false;
    }
}
