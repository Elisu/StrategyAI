using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanUnit : Unit
{
    public int Size { get; protected set;}

    public static float Speed { get; protected set; }

    public static GameObject UnitPrefab { get; protected set; }


}
