using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAStart : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform _sekeer, _target;

    private void Update()
    {
        FindPath(_sekeer.position, _target.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPos)
    {
        Node startNode = _grid.GetNodeFromWorldPoint(startPosition);
        Node targetNode = _grid.GetNodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in _grid.GetNeighbors(currentNode))
            {
                if (!neighbor.Walkable || closedSet.Contains(neighbor)) continue;

                int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        _grid.Path = path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        else return 14 * distX + 10 * (distY - distX);
    }
}
