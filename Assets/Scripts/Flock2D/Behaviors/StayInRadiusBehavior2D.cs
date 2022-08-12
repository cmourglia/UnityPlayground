using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behaviors/Stay in radius")]
public class StayInRadiusBehavior2D : FlockBehavior2D
{
    [SerializeField] Vector2 center;
    [SerializeField] float radius = 15f;

    public override Vector2 ComputeMove(FlockAgent2D agent, List<Transform> neighbors, Flock2D flock)
    {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;

        float t = centerOffset.magnitude / radius;

        if (t < 0.9f)
        {
            // We are not too far from the center
            return Vector2.zero;
        }

        return centerOffset * t * t;
    }
}
