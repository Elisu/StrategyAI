using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanUnit : Unit
{
    public int Size { get; protected set;}

    public float Speed { get; protected set; }

    public GameObject UnitPrefab { get; protected set; }


}
