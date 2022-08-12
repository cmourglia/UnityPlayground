using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behaviors/Composite")]
public class CompositeBehavior2D : FlockBehavior2D
{
    public List<FlockBehavior2D> behaviors;
    public List<float> weights;

    public override Vector2 ComputeMove(FlockAgent2D agent, List<Transform> neighbors, Flock2D flock)
    {
        if (weights.Count != behaviors.Count)
        {
            Debug.LogError($"Mismatch between behaviors and weights array in {name}", this);
            return Vector2.zero;
        }

        Vector2 sum = Vector2.zero;

        for (int i = 0; i < weights.Count; i += 1)
        {
            List<Transform> actualNeighbors = behaviors[i].filter != null
                ? behaviors[i].filter.FilterNeighbors(agent, neighbors)
                : neighbors;

            Vector2 move = behaviors[i].ComputeMove(agent, actualNeighbors, flock) * weights[i];

            if (move != Vector2.zero && move.sqrMagnitude > weights[i] * weights[i])
            {
                move = move.normalized * weights[i];
            }

            sum += move;
        }

        return sum;
    }
}