using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerUnit : Unit
{
    public int Health { get; protected set; }

    public int Damage { get; protected set; }

    public int Range { get; protected set; }

    public int Price { get; protected set; }

    public bool Passable => false;
}
