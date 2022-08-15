using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float maxSteeringForce = 5f;
    [SerializeField] float maxHealth = 10f;

    SpriteRenderer _spriteRenderer;

    float _health = 0f;

    Vector2 _velocity = Vector2.zero;
    Vector2 _acceleration = Vector2.zero;

    public FoodSpawner foodSpawner;
    public PopulationHandler populationHandler;

    public DNA dna;

    public float lifetime { get; private set; } = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _health = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        lifetime += Time.deltaTime;
        _health -= Time.deltaTime;

        _spriteRenderer.color = Color.Lerp(Color.green, Color.red, 1f - _health / maxHealth);

        if (_health <= 0f)
        {
            Die();
        }

        Behave(foodSpawner.goodFood, foodSpawner.badFood);

        Move(Time.deltaTime);
    }

    private void Behave(List<Food> goodFood, List<Food> badFood)
    {
        Vector2 steerGood = Steer(GetClosestFood(goodFood, dna.genes[2].Remap(0f, 1f, 0f, 25f)));
        Vector2 steerBad = Steer(GetClosestFood(badFood, dna.genes[3].Remap(0f, 1f, 0f, 25f)));

        ApplyForce(steerGood * dna.genes[0].Remap(0f, 1f, -2f, 2f));
        ApplyForce(steerBad * dna.genes[1].Remap(0f, 1f, -2f, 2f));
    }

    private Vector2 Steer(Vector2 target)
    {
        Vector2 currentPosition = (Vector2)transform.position;
        Vector2 desiredVelocity = (target - currentPosition).normalized * maxSpeed;

        Vector2 steering = Vector2.ClampMagnitude(desiredVelocity - _velocity, maxSteeringForce);

        return steering;
    }

    private void Move(float dt)
    {
        _velocity += _acceleration * dt;
        transform.position += (Vector3)_velocity * dt;
        transform.up = _velocity.normalized;

        _acceleration = Vector2.zero;
    }

    private void ApplyForce(Vector2 force)
    {
        _acceleration += force;
    }

    private Vector2 GetClosestFood(List<Food> foodList, float perceptionDistance)
    {
#if false
        float closestDist = float.MaxValue;
        Vector2 target = Vector2.zero;

        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 10f);

        foreach (Collider2D collider in colliders)
        {
            Food food = collider.gameObject.GetComponent<Food>();
            if (food?.type == foodType)
            {
                float dist = Vector2.SqrMagnitude(collider.transform.position - transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    target = collider.transform.position;
                }
            }
        }
#else
        Vector2 target = Vector2.zero;
        float closestDist = float.MaxValue;

        foreach (Food food in foodList)
        {
            float dist = Vector2.SqrMagnitude(transform.position - food.transform.position);
            if (dist < closestDist && dist < perceptionDistance * perceptionDistance)
            {
                closestDist = dist;
                target = (Vector2)food.transform.position;
            }
        }
#endif

        return target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Food food = collision.gameObject.GetComponent<Food>();

        if (food)
        {
            _health += foodSpawner.EatFood(food);
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        populationHandler.OnAgentDeath(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * dna.genes[1].Remap(0f, 1f, -2f, 2f) * 10f));
        Gizmos.DrawWireSphere(transform.position, dna.genes[3] * 25f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * dna.genes[0].Remap(0f, 1f, -2f, 2f) * 10f));
        Gizmos.DrawWireSphere(transform.position, dna.genes[2] * 25f);
        //Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
