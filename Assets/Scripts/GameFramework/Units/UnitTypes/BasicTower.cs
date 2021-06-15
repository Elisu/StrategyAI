using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : TowerUnit
{
    public BasicTower()
    {
        Health = BasicTowerSetup.Health;
        Damage = BasicTowerSetup.Damage;
        Range = BasicTowerSetup.Range;
    }
}
