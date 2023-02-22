using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    public readonly Army OwnArmy;
    public readonly Army EnemyArmy;
    public readonly IObjectMap Map;

    public GameInfo(Army own, Army enemy, IObjectMap map)
    {
        OwnArmy = own;
        EnemyArmy = enemy;
        Map = map;
    }
}
