using System.Collections.Generic;
using UnityEngine;

public class IObjectMap : Map<Field>
{
    internal float SizeMultiplier { get; private set; }

    public  IReadOnlyList<Vector2Int> CastleArea { get; private set; }

    private List<Field> spawns;
    Dictionary<Vector2Int, VisualController> structureObjects;
    Dictionary<Vector2Int, string> structureTags;

    internal IObjectMap(int height, int width, List<List<Transform>> realMap, float scale) : base(width, height)
    {
        SizeMultiplier = scale;
        spawns = new List<Field>();
        structureObjects = new Dictionary<Vector2Int, VisualController>();
        structureTags = new Dictionary<Vector2Int, string>();
        LoadMap(realMap);
    }

    private void LoadMap(List<List<Transform>> realMap)
    {
        List<Vector2Int> castle = new List<Vector2Int>();

        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                FieldInfo fieldInfo = realMap[j][i].GetComponent<FieldInfo>();
                Vector2Int position = new Vector2Int(i, j);
                Field field = new Field(fieldInfo, position);               

                map[i, j] = field;

                if (field != null)
                {
                    if (field.Square == SquareType.Building)
                    {
                       
                        Transform child = fieldInfo.transform.GetChild(0);
                        structureObjects.Add(position, child.GetComponent<VisualController>());
                        structureTags.Add(position, child.tag);
                    }

                    if (field.Square == SquareType.Spawn)
                        spawns.Add(map[i, j]);

                    if (field.Side == Role.Defender)
                        castle.Add(field.Position);
                }

            }

        CastleArea = castle.AsReadOnly();
    }

    internal void ReloadMap(Instance instance)
    {
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                if (map[i, j].OnField is TroopBase)
                    map[i, j].OnField = null;

                if (instance.IsTraining)
                {
                    if (structureTags.TryGetValue(map[i, j].Position, out string tag))
                        map[i, j].OnField = instance.GetArmy(Role.Defender).AddStructure(tag, map[i, j].Position, instance);
                }
                else
                {
                    if (structureObjects.TryGetValue(map[i, j].Position, out VisualController structure))
                        map[i, j].OnField = instance.GetArmy(Role.Defender).AddStructure(structure, map[i, j].Position, instance);
                }
            }
    }

    internal bool GetFreeSpawn(Role role, out Vector2Int spawnPos)
    {
        IObject spawn = spawns.Find(x => x.Side == role && x.CanPass(role) == true);

        if (spawn != null)
        {
            spawnPos = spawn.Position;
            return true;
        }

        spawnPos = new Vector2Int(-1, -1);
        return false;
    }

    public new Field this[Vector2Int index]
    {
        get
        {
            return map[index.x, index.y];
        }

        set
        {
            map[index.x, index.y] = value;
        }
    }

    public new Field this[int x, int y]
    {
        get
        {
           return map[x, y];
        }

        set
        {
            map[x, y] = value;
        }
    }

}
