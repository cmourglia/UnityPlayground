using System.Collections.Generic;
using UnityEngine;

public abstract class BoidBehavior : ScriptableObject
{
    public NeighborFilter filter;

    public abstract Vector2 ComputeBoidMove(Boid agent, List<Transform> neighbors, BoidSystem flock, Vector2 targetSpeed);
}