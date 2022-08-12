using System.Collections.Generic;
using UnityEngine;

public abstract class NeighborFilter : ScriptableObject
{
    public abstract List<Transform> FilterNeighbors(FlockAgent2D agent, List<Transform> neighbors);
}
