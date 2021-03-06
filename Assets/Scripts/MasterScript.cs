using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public static Map<IObject> map = new Map<IObject>(20,20);
    public static List<ITroop> defenderTroops = new List<ITroop>();
    public static List<ITroop> attackerTroops = new List<ITroop>();
    public static Queue<Tuple<IMovable, Queue<Vector2Int>>> moving = new Queue<Tuple<IMovable, Queue<Vector2Int>>>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < map.Width; i++)
            for (int j = 0; j < map.Height; j++)
            {
                map[i, j] = new Grass(); 
            }
    }

    void FixedUpdate()
    {
        int count = moving.Count;
        for (int i = 0; i < count; i++)
        {
            Debug.Log(string.Format("Moving {0}xtimes", moving.Count));
            Tuple<IMovable, Queue<Vector2Int>> movingObject = moving.Dequeue();
            Vector2Int nextPos = movingObject.Item2.Peek();
            if (map[nextPos] == null || map[nextPos].Passable == true)
            {
                map[movingObject.Item1.Position] = new Grass();
                Vector2 next = new Vector2(nextPos.x - movingObject.Item1.Position.x, nextPos.y - movingObject.Item1.Position.y);
                movingObject.Item1.ActualPosition += next * movingObject.Item1.Speed;
                //if (Vector2.Distance(movingObject.Item1.ActualPosition, next) < movingObject.Item1.Speed)
                if (movingObject.Item1.Position == nextPos)
                    movingObject.Item2.Dequeue();
                map[movingObject.Item1.Position] = movingObject.Item1;
                if (movingObject.Item2.Count > 0)
                    moving.Enqueue(movingObject);
            }
            else
            {
                //TO DO recompute route
            }

        }
    }
}
