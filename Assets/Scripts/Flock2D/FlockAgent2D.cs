using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent2D : MonoBehaviour
{
    public Collider2D agentCollider { get; set; }
    public Flock2D flockManager { get; private set; }

    public void Initialize(Flock2D flockManager)
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