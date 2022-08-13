using System.Collections.Generic;
using UnityEngine;

public class DebugDrawer : MonoBehaviour
{
    struct RayInfo
    {
        public Vector3 origin;
        public Vector3 direction;
        public Color color;
    }

    public static DebugDrawer instance { get; private set; } = null;

    List<RayInfo> rays = new();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of DebugDrawer in the scene. There must be only one.");
            return;
        }

        instance = this;
    }

    public void DrawRay(Vector3 origin, Vector3 direction, Color color)
    {
        rays.Add(new RayInfo { origin = origin, direction = direction, color = color, });
    }

    private void OnDrawGizmos()
    {
        foreach (RayInfo ray in rays)
        {
            Gizmos.color = ray.color;
            Gizmos.DrawRay(ray.origin, ray.direction);
        }

        rays.Clear();
    }
}
