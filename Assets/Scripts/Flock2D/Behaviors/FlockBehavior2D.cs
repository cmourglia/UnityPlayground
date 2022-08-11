using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior2D : ScriptableObject
{
    public abstract Vector2 ComputeMove(FlockAgent2D agent, List<Transform> neighbors, Flock2D flock);
}