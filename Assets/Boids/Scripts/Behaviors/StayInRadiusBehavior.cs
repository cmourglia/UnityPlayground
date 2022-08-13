using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Stay in radius")]
public class StayInRadiusBehavior : BoidBehavior
{
    [SerializeField] Vector2 center;
    [SerializeField] float radius = 15f;

    public override Vector2 ComputeBoidMove(Boid agent, List<Transform> neighbors, BoidSystem boidSystem, Vector2 targetSpeed)
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
