using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : IObject
{
    public Vector2Int Position { get; private set; }

    public SquareType Square { get; private set; }

    public IObject OnField
    {
        get => onField;

        internal set
        {
            if (onField is TroopBase && value == null)
                onField = preserve;
            else if (onField is Gate && value is TroopBase)
            {
                preserve = onField;
                onField = value;
            }
            else
                onField = value;
            
        }
    }

    private Role side;
    private IObject onField;
    private IObject preserve;

    public Field(FieldInfo info, Vector2Int position)
    {
        Square = info.Square;
        side = info.Side;
        Position = position;
    }

    public Role Side => side;

    public bool CanPass(Role role)
    {
        if (OnField != null)
            return OnField.CanPass(role);
        else
            return true;
    }
}
