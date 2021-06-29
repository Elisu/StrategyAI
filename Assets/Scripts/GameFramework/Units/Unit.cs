using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Unit 
{
    public int Health { get; }

    public int Damage { get; }

    public int ReloadRate { get; }

    public int Range { get; }

    public int Price { get; }

    public bool Passable { get;}

}
