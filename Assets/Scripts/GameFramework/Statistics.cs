using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Statistics 
{
    public int dealtDamage { get; private set; }
    public int receivedDamage { get; private set; }
    public int killedEnemies { get; private set; }
    public int destroyedBuildings { get; private set; }

    public Statistics(int damageDealt, int damageReceived, int enemyKilled, int buildDestroyed)
    {
        dealtDamage = damageDealt;
        receivedDamage = damageReceived;
        killedEnemies = enemyKilled;
        destroyedBuildings = buildDestroyed;
    }

    public static Statistics operator+ (Statistics a, Statistics b)
    {
        Statistics sum = new Statistics(a.dealtDamage + b.dealtDamage,
                                        a.receivedDamage + b.receivedDamage,
                                        a.killedEnemies + b.killedEnemies,
                                        a.destroyedBuildings + b.destroyedBuildings);

        return sum;
    }

}
