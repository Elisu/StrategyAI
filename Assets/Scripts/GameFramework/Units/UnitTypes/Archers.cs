using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archers : HumanUnit
{
    public Archers()
    {
        Health = ArchersSetup.Health;
        Speed = ArchersSetup.Speed;
        Damage = ArchersSetup.Damage;
        Range = ArchersSetup.Range;
        UnitPrefab = ArchersSetup.UnitPrefab;
        BundleCount = 30;
    }
    public override float GetDefenseAgainst(Type unitType)
    {
        switch(unitType.Name)
        {
            case nameof(Archers):
                return 1;
            case nameof(Cavalry):
                return 0.8f;
            case nameof(Swordsmen):
                return 0.75f;
        }

        if (unitType.IsSubclassOf(typeof(TowerBase)))
            return 0.4f;

        return 0.7f;
    }

    internal override bool GiveDamage(Damageable enemy, int totalDamage)
    {
        if (enemy is TroopBase)
        {
            TroopBase enemyTroop = (TroopBase)enemy;

            while (totalDamage > 0)
            {
                int index = rnd.Next(0, enemyTroop.Count);
                if (enemyTroop.TakeDamage(Damage, index, 1))
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
