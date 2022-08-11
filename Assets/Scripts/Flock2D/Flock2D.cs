﻿using System.Collections.Generic;
using UnityEngine;

public class Flock2D : MonoBehaviour
{
    [SerializeField] FlockAgent2D agentPrefab;
    [SerializeField] FlockBehavior2D behavior;

    [Range(1, 500), SerializeField] int agentCount;

    [Range(1f, 100f), SerializeField] float driveFactor = 10f;
    [Range(1f, 100f), SerializeField] float maxSpeed = 5f;

    [Range(1f, 50f), SerializeField] float visionRadius = 5f;
    [Range(0f, 1f), SerializeField] float avoidanceRadiusRatio = 0.5f;

    const float AGENT_DENSITY = 0.08f;

    List<FlockAgent2D> agents = new();

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
            Vector2 agentSpawnLocation = Random.insideUnitCircle * (float)agentCount * AGENT_DENSITY;
            float angle = Random.Range(0f, 360f);
            var rotation = Quaternion.Euler(Vector3.forward * angle);

            FlockAgent2D flockAgent = Instantiate(agentPrefab, agentSpawnLocation, rotation, transform);
            agents.Add(flockAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent2D agent in agents)
        {
            List<Transform> neighbors = GetNeighbors(agent);

            agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, neighbors.Count / 6f);

            Vector2 speed = behavior.ComputeMove(agent, neighbors, this);
            speed *= driveFactor;

            if (speed.sqrMagnitude > sqrMaxSpeed)
            {
                speed = speed.normalized * maxSpeed;
            }

            agent.Move(speed);
        }
    }

    List<Transform> GetNeighbors(FlockAgent2D agent)
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