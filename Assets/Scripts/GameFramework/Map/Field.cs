using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : IObject
{
    public Vector2Int Position { get; private set; }

    public SquareType Square { get; private set; }

    public IObject OnField { get; internal set; }

    private Role side;

    public Field (FieldInfo info, Vector2Int position)
    {
        Square = info.Square;
        side = info.Side;
        Position = position;
    }

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

    public bool CanPass(Role role)
    {
        if (OnField != null)
            return OnField.CanPass(role);
        else
            return true;
    }
}
