using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : Damageable, IRecruitable
{
    public abstract Statistics GetStats();
}
