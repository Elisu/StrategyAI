using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    public readonly Army OwnArmy;
    public readonly Army EnemyArmy;

    public GameInfo(Army own, Army enemy)
    {
        OwnArmy = own;
        EnemyArmy = enemy;
    }
}
