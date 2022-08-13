using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Align")]
public class AlignBehavior : BoidBehavior
{
    public override Vector2 ComputeBoidMove(Boid boid, List<Transform> neighbors, BoidSystem boidSystem, Vector2 targetSpeed)
    {
        if (neighbors.Count == 0)
        {
            return boid.transform.up;
        }

        Vector2 move = Vector2.zero;

        foreach (Transform neighbor in neighbors)
        {
            move += (Vector2)neighbor.transform.up;
        }

        move /= neighbors.Count;

        return move;
    }
}