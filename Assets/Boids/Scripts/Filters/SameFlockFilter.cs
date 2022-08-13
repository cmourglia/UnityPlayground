using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Filters/Same flock")]
public class SameFlockFilter : NeighborFilter
{
    public override List<Transform> FilterNeighbors(Boid agent, List<Transform> neighbors)
    {
        List<Transform> filteredNeighbors = new(neighbors.Count);

        foreach (Transform t in neighbors)
        {
            if (t.GetComponent<Boid>()?.flockManager == agent.flockManager)
            {
                filteredNeighbors.Add(t);
            }
        }

        return filteredNeighbors;
    }
}
