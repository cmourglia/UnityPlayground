using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;
    [SerializeField] private bool onlyPath = true;

    public List<Vector3> searchedPath;

    private Node[,] grid;
    private float nodeDiameter;

    public int GridSizeX { get; private set; }
    public int GridSizeY { get; private set; }

    private void Start() {
        nodeDiameter = nodeRadius * 2.0f;
        GridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        GridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid == null) return;

        if (searchedPath?.Count > 0) {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(searchedPath[0], nodeRadius);
            for (var i = 1; i < searchedPath.Count; ++i) {
                Gizmos.DrawLine(searchedPath[i - 1], searchedPath[i]);
                Gizmos.DrawSphere(searchedPath[i], nodeRadius);
            }
        }

        if (onlyPath) return;

        foreach (Node n in grid) {
            Gizmos.color = n.walkable ? Color.magenta : Color.red;
            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
        }
    }

    private void CreateGrid() {
        grid = new Node[GridSizeX, GridSizeY];

        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2.0f -
                             Vector3.forward * gridWorldSize.y / 2.0f;

        for (var x = 0; x < GridSizeX; ++x) {
            for (var y = 0; y < GridSizeY; ++y) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, new Vector2Int(x, y));
            }
        }
    }

    public void ResetNodes() {
        foreach (Node n in grid) {
            n.parent = null;
            n.state = NodeState.None;
            n.gCost = int.MaxValue;
            n.hCost = int.MaxValue;
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition) {
        Vector2Int coords = CoordsFromWorldPosition(worldPosition);
        return grid[coords.x, coords.y];
    }

    private Vector2Int CoordsFromWorldPosition(Vector3 worldPosition) {
        float pctX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2.0f) / gridWorldSize.x);
        float pctY = Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2.0f) / gridWorldSize.y);

        int x = Mathf.RoundToInt(pctX * (GridSizeX - 1));
        int y = Mathf.RoundToInt(pctY * (GridSizeY - 1));

        return new Vector2Int(x, y);
    }

    public List<Node> GetNeighbors(Node node) {
        var list = new List<Node>(8);

        Vector2Int coords = CoordsFromWorldPosition(node.worldPosition);

        for (int i = coords.x - 1; i <= coords.x + 1; ++i) {
            if (i < 0 || i >= GridSizeX) continue;

            for (int j = coords.y - 1; j <= coords.y + 1; ++j) {
                if (j < 0 || j >= GridSizeY) continue;
                if (i == coords.x && j == coords.y) continue;

                Node n = grid[i, j];
                if (n.walkable) list.Add(grid[i, j]);
            }
        }

        return list;
    }
}
