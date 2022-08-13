using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/Behaviors/Avoid obstacles")]
class AvoidObstaclesBehavior : BoidBehavior
{
    [SerializeField] int maxRayCount = 36;
    [SerializeField, Range(0f, 10f)] float rayLength = 1f;
    [SerializeField] LayerMask mask;

    public override Vector2 ComputeBoidMove(Boid agent, List<Transform> neighors, BoidSystem flockManager, Vector2 targetSpeed)
    {
        _ = neighors;
        _ = flockManager;

        float deltaAngle = 360f / (maxRayCount - 1);
        float mult = 1f;

        var origin = (Vector2)agent.transform.position;
        var speed = Vector2.zero;

        float angleOffset = 0f;

        Vector3 desiredDirection = (Vector3)targetSpeed.normalized;

        for (int i = 0; i < maxRayCount; ++i)
        {
            float angle = angleOffset * mult;
            var orientation = Quaternion.AngleAxis(angle, Vector3.forward);

            Vector2 direction = (Vector2)(orientation * desiredDirection);

            RaycastHit2D raycast = Physics2D.Raycast(origin, direction, rayLength, mask);
#if UNITY_EDITOR
            DebugDrawer.instance.DrawRay(origin, direction, raycast ? Color.red : Color.green);
#endif
            if (!raycast)
            {
                speed = direction;
                break;
            }

            if (i % 2 == 0)
            {
                angleOffset += deltaAngle;
            }
            mult *= -1f;
        }

        return speed.normalized * targetSpeed.magnitude;
    }
}
