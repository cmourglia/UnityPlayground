using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Align behavior")]
public class AlignBehavior2D : FlockBehavior2D
{
    public override Vector2 ComputeMove(FlockAgent2D agent, List<Transform> neighbors, Flock2D _)
    {
        if (neighbors.Count == 0)
        {
            return agent.transform.up;
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