using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map<T>
{
    private T[,] map;
    private GameObject[,] realMap;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Map(int width, int height, GameObject mapObejct = null)
    {
        map = new T[width, height];
        Width = width;
        Height = height;

        if (mapObejct != null)
        {

        }
    }

    public T this[Vector2Int index]
    {
        get => map[index.x, index.y];
        set => map[index.x,index.y] = value;
    }

    public T this[int x, int y]
    {
        get => map[x, y];
        set => map[x, y] = value;
    }


}
