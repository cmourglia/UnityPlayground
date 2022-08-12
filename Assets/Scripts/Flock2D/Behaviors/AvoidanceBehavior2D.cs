using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behaviors/Avoidance")]
public class AvoidanceBehavior2D : FlockBehavior2D
{
    public override Vector2 ComputeMove(FlockAgent2D agent, List<Transform> neighbors, Flock2D flock)
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