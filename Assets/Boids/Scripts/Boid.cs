using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Boid : MonoBehaviour
{
    public Collider2D agentCollider { get; set; }
    public BoidSystem flockManager { get; private set; }

    public void Initialize(BoidSystem flockManager)
    {
        this.flockManager = flockManager;
    }

    private void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}