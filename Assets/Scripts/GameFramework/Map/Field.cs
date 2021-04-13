using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Field : MonoBehaviour, IObject
{
    public SquareType square;

    [SerializeField]
    private Role side;

    public Vector2Int Position { get; internal set; }

    public IObject OnField { get; internal set; }

    public Role Side
    {
        get
        {
            if (OnField == null)
                return side;
            else
            {
                if (OnField is IRecruitable recruit)
                    return recruit.Side;
                else
                    return Role.Neutral;
            }
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