using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IMappable
{
    public static int STRAIGHT_COST = 10;
    public static int DIAGONAL_COST = 14;
    public int FromStartCost { get; set; }

    public int ToTargetCost { get; set; }

    public int TotalCost { get => FromStartCost + ToTargetCost; }

    public Vector2Int Position { get; set; }

    public Node previous = null;

    public bool visited = false;

    public Node(Vector2Int pos, int G)
    {
        this.FromStartCost = G;
        Position = pos;
    }
}
public class Pathfinding
{ 

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        //Debug.Log("Looking for path");
        List<Node> opened = new List<Node>();
        Map<Node> pathMap = new Map<Node>(MasterScript.map.Width, MasterScript.map.Height);

        for (int i = 0; i < pathMap.Width; i++)
            for (int j = 0; j < pathMap.Height; j++)
            {
                pathMap[i, j] = new Node(new Vector2Int(i, j), int.MaxValue);
            }

        pathMap[start].FromStartCost = 0;
        pathMap[start].ToTargetCost = CalculateDistance(start, target);
        pathMap[start].visited = true;
        opened.Add(pathMap[start]);

        while (opened.Count > 0)
        {
            Node currentNode = FindBestNode(opened);
            if (currentNode.Position == target)
                return CalculatePath(currentNode);

            opened.Remove(currentNode);
            currentNode.visited = true;

            for (int i = Mathf.Max(0, currentNode.Position.x - 1); i < Mathf.Min(pathMap.Width, currentNode.Position.x + 2); i++)
                for (int j = Mathf.Max(0, currentNode.Position.y - 1); j < Mathf.Min(pathMap.Height, currentNode.Position.y + 2); j++)
                {
                    if (i == currentNode.Position.x || j == currentNode.Position.y)
                    {
                        if (MasterScript.map[i, j].Passable == true)
                        {
                            if (pathMap[i, j].visited == false)
                            {
                                int cumulatedCost = currentNode.FromStartCost + CalculateDistance(new Vector2Int(i, j), currentNode.Position);
                                if (cumulatedCost < pathMap[i, j].FromStartCost)
                                {
                                    pathMap[i, j].ToTargetCost = CalculateDistance(new Vector2Int(i, j), target);
                                    pathMap[i, j].FromStartCost = cumulatedCost;
                                    pathMap[i, j].previous = currentNode;

                                    if (!opened.Contains(pathMap[i, j]))
                                        opened.Add(pathMap[i, j]);
                                }

                            }
                        }
                    }
                                        
                   
                    
                }
                    
        }

        Debug.LogWarning("Path not found");
        return null;

    }

    private static List<Vector2Int> CalculatePath(Node node)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        while (node.previous != null)
        {
            path.Add(node.Position);
            node = node.previous;
        }

        path.Reverse();

        return path;
    }

    private static int CalculateDistance(Vector2Int from, Vector2Int to)
    {
        int xDistance = Mathf.Abs(from.x - to.x);
        int yDistance = Mathf.Abs(from.y - to.y);
        int diff = Mathf.Abs(xDistance - yDistance);
        return Node.DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + Node.STRAIGHT_COST * diff;
    }

    private static Node FindBestNode(List<Node> nodes)
    {
        Node bestNode = nodes[0];

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].TotalCost < bestNode.TotalCost)
                bestNode = nodes[i];

        }

        return bestNode;
    }

}


        

    



    

