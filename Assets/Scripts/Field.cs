using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour, IObject
{
    public SquareType square;

    [SerializeField]
    private Role side;

    public Vector2Int Position { get; set; }

    public IObject OnField { get; set; }

    public Role Side
    {
        get
        {
            if (OnField == null)
                return side;
            else
                return OnField.Side;
        }
    }

    public bool Passable
    {
        get
        {
            if (OnField != null)
                return OnField.Passable;
            else
                return true;
        }
    }

    public int Size { get; }
}