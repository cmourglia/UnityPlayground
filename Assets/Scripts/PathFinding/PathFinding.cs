using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathFinding : MonoBehaviour {
    private static readonly ProfilerMarker s_ResetNodesPerfMarker = new("PathFinding.ResetNodes");
    private static readonly ProfilerMarker s_SearchPathPerfMarker = new("PathFinding.SearchPath");
    private static readonly ProfilerMarker s_RemoveFirstNodeProfilerMarker = new("PathFinding.RemoveFirstNode");
    private static readonly ProfilerMarker s_AddNodeProfilerMarker = new("PathFinding.AddNode");
    private static readonly ProfilerMarker s_GetNeighborsMarker = new("PathFinding.GetNeighbors");

    [SerializeField] private Transform predator;
    [SerializeField] private Transform prey;

    private Grid grid;

    private void Awake() {
        grid = GetComponent<Grid>();
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            s_ResetNodesPerfMarker.Begin();
            grid.ResetNodes();
            s_ResetNodesPerfMarker.End();

            s_SearchPathPerfMarker.Begin();
            grid.searchedPath = FindPath(grid, predator.position, prey.position);
            s_SearchPathPerfMarker.End();
        }
    }


    private static List<Vector3> FindPath(Grid grid, Vector3 startPosition, Vector3 targetPosition) {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        Node startNode = grid.NodeFromWorldPosition(startPosition);
        Node targetNode = grid.NodeFromWorldPosition(targetPosition);

        int GetDistance(Node start, Node end) {
            int distanceX = Mathf.Abs(start.gridCoords.x - end.gridCoords.x);
            int distanceY = Mathf.Abs(start.gridCoords.y - end.gridCoords.y);

            int maxDistance = Math.Max(distanceX, distanceY);
            int minDistance = Math.Min(distanceX, distanceY);

            return 14 * minDistance + 10 * (maxDistance - minDistance);
        }

        int D(Node currentNode) {
            return GetDistance(startNode, currentNode);
        }

        int H(Node currentNode) {
            return GetDistance(currentNode, targetNode);
        }

        var openNodes = new BinaryHeap<Node>(grid.GridSizeX * grid.GridSizeY);

        openNodes.Add(startNode);
        startNode.state = NodeState.Open;
        startNode.gCost = 0;
        startNode.hCost = H(startNode);

        List<Vector3> result = null;

        while (openNodes.Count > 0) {
            // FIXME: Check how we find the smallest value
            s_RemoveFirstNodeProfilerMarker.Begin();
            Node current = openNodes.Pop();
            s_RemoveFirstNodeProfilerMarker.End();

            if (current == targetNode) {
                result = BuildPath(targetNode);
                break;
            }

            current.state = NodeState.Closed;

            s_GetNeighborsMarker.Begin();
            var neighbors = grid.GetNeighbors(current);
            s_GetNeighborsMarker.End();

            foreach (Node neighbor in neighbors) {
                int gCost = current.gCost + D(neighbor);
                if (gCost >= neighbor.gCost) continue;

                neighbor.gCost = gCost;
                neighbor.hCost = H(neighbor);
                neighbor.parent = current;
                if (neighbor.state == NodeState.None) {
                    neighbor.state = NodeState.Open;
                    s_AddNodeProfilerMarker.Begin();
                    openNodes.Add(neighbor);
                    s_AddNodeProfilerMarker.End();
                }
            }
        }

        stopwatch.Stop();
        Debug.Log($"Path computed in {stopwatch.ElapsedMilliseconds} ms");

        return result;
    }

    private static List<Vector3> BuildPath(Node finalNode) {
        var nodes = new List<Node>();

        Node current = finalNode;

        while (current != null) {
            nodes.Add(current);
            current = current.parent;
        }

        nodes.Reverse();

        return SimplifyPath(nodes);
    }

    private static List<Vector3> SimplifyPath(List<Node> nodes) {
        if (nodes.Count < 2) {
            Debug.LogError("Path with only one node does not make sense");
            return null;
        }

        var waypoints = new List<Vector3>();
        waypoints.Add(nodes[0].worldPosition);

        Node lastNode = nodes[1];
        Vector2Int dir = nodes[1].gridCoords - nodes[0].gridCoords;

        for (var i = 2; i < nodes.Count; ++i) {
            Node node = nodes[i];
            Vector2Int newDir = node.gridCoords - lastNode.gridCoords;

            if (newDir.x != dir.x || newDir.y != dir.y) {
                waypoints.Add(lastNode.worldPosition);
            }

            dir = newDir;
            lastNode = node;
        }

        waypoints.Add(nodes[^1].worldPosition);

        return waypoints;
    }
}
