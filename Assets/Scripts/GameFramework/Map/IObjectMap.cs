using System.Collections.Generic;
using UnityEngine;

internal class IObjectMap : Map<Field>
{
    public float SizeMultiplier { get; protected set; }

    private List<Field> spawns;

    List<List<Transform>> MapPrefab;

    public IObjectMap(int height, int width, List<List<Transform>> realMap = null) : base(width, height)
    {
        if (realMap == null)
            return;

        SizeMultiplier = realMap[0][0].localScale.x;
        spawns = new List<Field>();
        MapPrefab = realMap;
        ReloadMap();
    }

    public void ReloadMap()
    {
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                Field field = MapPrefab[j][i].GetComponent<Field>();
                field.Position = new Vector2Int(i, j);
                map[i, j] = field;


                if (field != null)
                    if (field.square == SquareType.Spawn)
                        spawns.Add(map[i, j]);
            }
    }

    public Vector2Int GetFreeSpawn(Role role)
    {
        IObject spawn = spawns.Find(x => x.Side == role && x.Passable == true);

        if (spawn != null)
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
            if (x >= Width || y >= Height)
                Debug.LogWarning(string.Format("x {0}  y {1}", x, y));

            if (map[x, y].OnField != null)
                return map[x, y].OnField;
            else
                return map[x, y];
        }

        set => map[x, y].OnField = value;
    }

}
