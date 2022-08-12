using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behaviors/Steered cohesion")]
public class SteeredCohesionBehavior : FlockBehavior2D
{
    [SerializeField] float agentSmoothTime = 0.5f;

    Vector2 currentVelocity;

    public override Vector2 ComputeMove(FlockAgent2D agent, List<Transform> neighbors, Flock2D _)
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
        move = Vector2.SmoothDamp(agent.transform.up, move, ref currentVelocity, agentSmoothTime);

        return move;
    }
}
