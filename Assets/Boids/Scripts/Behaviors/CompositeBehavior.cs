using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Composite")]
public class CompositeBehavior : BoidBehavior
{
    public List<BoidBehavior> behaviors = new();
    public List<float> weights = new();

    public override Vector2 ComputeBoidMove(Boid agent, List<Transform> neighbors, BoidSystem boidSystem, Vector2 targetSpeed)
    {
        if (weights.Count != behaviors.Count)
        {
            Debug.LogError($"Mismatch between behaviors and weights array in {name}", this);
            return Vector2.zero;
        }

        Vector2 boidSpeed = Vector2.zero;

        for (int i = 0; i < weights.Count; i += 1)
        {
            List<Transform> actualNeighbors = behaviors[i].filter != null
                ? behaviors[i].filter.FilterNeighbors(agent, neighbors)
                : neighbors;

            Vector2 move = behaviors[i].ComputeBoidMove(agent, actualNeighbors, boidSystem, boidSpeed) * weights[i];

            if (move != Vector2.zero && move.sqrMagnitude > weights[i] * weights[i])
            {
                move = move.normalized * weights[i];
            }

            boidSpeed += move;
        }

        return boidSpeed;
    }
}