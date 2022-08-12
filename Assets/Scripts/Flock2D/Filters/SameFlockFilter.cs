using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Filters/Same flock")]
public class SameFlockFilter : NeighborFilter
{
    public override List<Transform> FilterNeighbors(FlockAgent2D agent, List<Transform> neighbors)
    {
        List<Transform> filteredNeighbors = new(neighbors.Count);

        foreach (Transform t in neighbors)
        {
            if (t.GetComponent<FlockAgent2D>()?.flockManager == agent.flockManager)
            {
                filteredNeighbors.Add(t);
            }
        }

        return filteredNeighbors;
    }
}
