using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : IObject
{
    public Vector2Int Position => throw new System.NotImplementedException();

    public Role Side => Role.Neutral;

    public bool Passable { get => true; }

    public int Size { get => 50; }

}
