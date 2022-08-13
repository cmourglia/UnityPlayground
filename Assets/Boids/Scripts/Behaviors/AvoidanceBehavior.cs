using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Avoidance")]
public class AvoidanceBehavior : BoidBehavior
{
    public override Vector2 ComputeBoidMove(Boid agent, List<Transform> neighbors, BoidSystem flock, Vector2 targetSpeed)
    {
        if (neighbors.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;

        int avoidedCount = 0;

        foreach (Transform neighbor in neighbors)
        {
            Vector2 diff = agent.transform.position - neighbor.position;
            if (diff.sqrMagnitude < flock.sqrAvoidanceRadius)
            {
                avoidedCount += 1;
                move += (Vector2)diff;
            }
        }

        if (avoidedCount > 0)
        {
            move /= avoidedCount;
        }

        return move;
    }
}