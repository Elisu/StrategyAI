using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public Tower()
    {
        Health = TowerSetup.Health;
        Damage = TowerSetup.Damage;
        Defense = TowerSetup.Defense;
        Range = TowerSetup.Range;
    }
}
