using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent2D : MonoBehaviour
{
    public Collider2D agentCollider { get; set; }

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