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

    public override float GetDefenseAgainst(Type unitType)
    {
        switch (unitType.Name)
        {
            case nameof(Archers):
                return 0.95f;
            case nameof(Cavalry):
                return 0.6f;
            default:
                return 0.7f;
        }
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
