using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;

    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;

    public Node Parent;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }
}
