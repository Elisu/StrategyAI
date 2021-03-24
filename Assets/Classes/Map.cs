using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map<T> where T : IMappable
{
    protected T[,] map;

    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public Map(int height, int width)
    {
        map = new T[height, width];
        Width = width;
        Height = height;
    }

    public T this[Vector2Int index]
    {
        get => map[index.y, index.y];
        set => map[index.y,index.x] = value;
    }

    public T this[int x, int y]
    {
        get => map[x, y];
        set => map[x, y] = value;
    }

}
