using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Cohesion")]
public class CohesionBehavior : BoidBehavior
{
    public override Vector2 ComputeBoidMove(Boid agent, List<Transform> neighbors, BoidSystem boidSystem, Vector2 targetSpeed)
    {
        Vector2 move = Vector2.zero;

        if (neighbors.Count == 0)
        {
            return move;
        }

        foreach (Transform neighbor in neighbors)
        {
            move += (Vector2)neighbor.transform.position;
        }

        move /= neighbors.Count;

        move -= (Vector2)agent.transform.position;

        return move;
    }
}