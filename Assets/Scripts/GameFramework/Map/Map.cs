using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map<T> where T : IMappable
{
    protected T[,] map;

    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public Map(int width, int height)
    {
        map = new T[width, height];
        Width = width;
        Height = height;
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
