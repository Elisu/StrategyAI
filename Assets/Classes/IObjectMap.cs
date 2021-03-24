using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObjectMap : Map<Field>
{
    public float SizeMultiplier { get; protected set; }

    private List<Field> spawns;

    public IObjectMap (int height, int width, List<List<Transform>> realMap = null) : base(height, width)
    {
        if (realMap == null)
            return;

        SizeMultiplier = realMap[0][0].localScale.x;
        spawns = new List<Field>();

        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                Field field = realMap[i][j].GetComponent<Field>();
                map[i, j] = field;

                if (field != null)
                    if (field.square == SquareType.Spawn)
                        spawns.Add(map[i, j]);                    
            }
    }

    public Vector2Int GetFreeSpawn(Role role)
    {
        IObject spawn = spawns.Find(x => x.Side == role);

        if (spawn != null && spawn.Passable)
        {
            return spawn.Position;
        }

        return new Vector2Int(-1, -1);
    }

    public new IObject this[Vector2Int index]
    {
        get
        {
            if (map[index.x, index.y].OnField != null)
                return map[index.x, index.y].OnField;
            else
                return map[index.x, index.y];
        }

        set => map[index.x, index.y].OnField = value;
    }

    public new IObject this[int x, int y]
    {
        get
        {
            if (map[x, y].OnField != null)
                return map[x, y].OnField;
            else
                return map[x, y];
        }

        set => map[x, y].OnField = value;
    }

}
