using System.Collections.Generic;
using UnityEngine;

public class BoidSystem : MonoBehaviour
{
    [SerializeField] Boid agentPrefab;
    [SerializeField] BoidBehavior behavior;

    [Range(1, 500), SerializeField] int agentCount;

    [Range(1f, 100f), SerializeField] float driveFactor = 10f;
    [Range(1f, 100f), SerializeField] float maxSpeed = 5f;

    [Range(1f, 50f), SerializeField] float visionRadius = 5f;
    [Range(0f, 1f), SerializeField] float avoidanceRadiusRatio = 0.5f;

    const float AGENT_DENSITY = 0.08f;

    List<Boid> agents = new();

    public float sqrMaxSpeed { get; private set; }
    public float sqrVisionRadius { get; private set; }
    public float sqrAvoidanceRadius { get; private set; }

    private void Start()
    {
        sqrMaxSpeed = maxSpeed * maxSpeed;
        sqrVisionRadius = visionRadius * visionRadius;
        sqrAvoidanceRadius = avoidanceRadiusRatio * avoidanceRadiusRatio * sqrVisionRadius;

        for (int i = 0; i < agentCount; i += 1)
        {
            Vector2 agentSpawnLocation = Random.insideUnitCircle;
            float angle = Random.Range(0f, 360f);
            var rotation = Quaternion.Euler(Vector3.forward * angle);

            Boid flockAgent = Instantiate(agentPrefab, agentSpawnLocation, rotation, transform);
            flockAgent.Initialize(this);

            agents.Add(flockAgent);
        }
    }

    private void Update()
    {
        foreach (Boid agent in agents)
        {
            List<Transform> neighbors = GetNeighbors(agent);

            Vector2 speed = behavior.ComputeBoidMove(agent, neighbors, this, Vector2.zero);
            speed *= driveFactor;

            if (speed.sqrMagnitude > sqrMaxSpeed)
            {
                speed = speed.normalized * maxSpeed;
            }

            agent.Move(speed);
        }
    }

    List<Transform> GetNeighbors(Boid agent)
    {
        List<Transform> neighbors = new();

        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(agent.transform.position, visionRadius);

        foreach (Collider2D neighborCollider in neighborColliders)
        {
            if (neighborCollider != agent.agentCollider)
            {
                neighbors.Add(neighborCollider.transform);
            }
        }

        return neighbors;
    }
}
