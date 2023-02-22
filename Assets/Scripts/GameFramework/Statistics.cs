using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Statistics 
{
    public int dealtDamage { get; private set; }
    public int receivedDamage { get; private set; }
    public int killedEnemies { get; private set; }
    public int destroyedBuildings { get; private set; }
    public Type UnitType { get; private set; }

    public Statistics(int damageDealt, int damageReceived, int enemyKilled, int buildDestroyed, Type type)
    {
        dealtDamage = damageDealt;
        receivedDamage = damageReceived;
        killedEnemies = enemyKilled;
        destroyedBuildings = buildDestroyed;
        UnitType = type;
    }

    public Statistics(Statistics s)
    {
        dealtDamage = s.dealtDamage;
        receivedDamage = s.receivedDamage;
        killedEnemies = s.killedEnemies;
        destroyedBuildings = s.destroyedBuildings;
        UnitType = s.UnitType;
    }
}
