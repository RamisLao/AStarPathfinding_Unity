using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private LayerMask _unwalkableMask;
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeRadius;
    [SerializeField] private bool _showPath;

    private Node[,] _grid;
    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY;

    public List<Node> Path { get; set; }

    private void Start()
    {
        _nodeDiameter = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius)
                                     + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableMask));

                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z + _gridWorldSize.y / 2) / _gridWorldSize.y);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < _gridSizeX &&
                    checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbors.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));

        if (_grid != null)
        {
            foreach(Node node in _grid)
            {
                Gizmos.color = (node.Walkable) ? Color.white : Color.red;
                if (_showPath && Path != null)
                {
                    if (Path.Contains(node)) Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(node.WorldPosition, new Vector3(_nodeDiameter - 0.1f, 0.1f, _nodeDiameter - 0.1f));
            }
        }
    }
}
