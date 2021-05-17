using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archers : HumanUnit
{
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
}
