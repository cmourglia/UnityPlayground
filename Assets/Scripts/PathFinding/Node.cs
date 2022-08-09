using UnityEngine;

public enum NodeState {
    None,
    Open,
    Closed
}

public class Node : IHeapElement<Node> {
    public Node parent = null;

    public Vector3 worldPosition;
    public Vector2Int gridCoords;

    public int gCost = int.MaxValue;
    public int hCost = int.MaxValue;

    public NodeState state = NodeState.None;

    public readonly bool walkable;

    public Node(bool _walkable, Vector3 _worldPosition, Vector2Int _gridCoords) {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridCoords = _gridCoords;
    }

    private int FCost => hCost + gCost;

    public int CompareTo(Node other) {
        int cmp = FCost.CompareTo(other.FCost);
        if (cmp == 0) {
            cmp = hCost.CompareTo(other.hCost);
        }

        return -cmp;
    }

    public int HeapIndex { get; set; }
}
