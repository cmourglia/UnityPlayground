using System.Collections.Generic;
using UnityEngine;

public abstract class NeighborFilter : ScriptableObject
{
    public abstract List<Transform> FilterNeighbors(Boid agent, List<Transform> neighbors);
}
