using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Follow mouse")]
public class FollowMouseBehavior : BoidBehavior
{
    public override Vector2 ComputeBoidMove(Boid agent, List<Transform> neighbors, BoidSystem boidSystem, Vector2 targetSpeed)
    {
        return Vector2.zero;
    }
}
