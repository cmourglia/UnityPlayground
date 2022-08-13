using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float maxSteeringForce = 5f;
    [SerializeField] float maxHealth = 10f;

    float _maxSteeringForceSqr;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;

    Vector2 _target;

    float _lifetime = 0f;
    float _health = 0f;

    // Debug stuff
    Vector2 _steeringForce;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _maxSteeringForceSqr = maxSteeringForce * maxSteeringForce;

        _health = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        _target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = _rigidBody.velocity;

        _lifetime += Time.deltaTime;
        _health -= Time.deltaTime;

        _spriteRenderer.color = Color.Lerp(Color.red, Color.black, 1f - _health / maxHealth);

        if (_health <= 0f)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = (Vector2)transform.position;

        Vector2 desiredVelocity = (_target - currentPosition).normalized * maxSpeed;

        Vector2 steeringForce = desiredVelocity - _rigidBody.velocity;

        if (steeringForce.sqrMagnitude > _maxSteeringForceSqr)
        {
            steeringForce.Normalize();
            steeringForce *= maxSteeringForce;
        }

        _rigidBody.AddForce(steeringForce);
        _steeringForce = steeringForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Food food = collision.gameObject.GetComponent<Food>();

        if (!food)
        {
            Debug.LogError($"Collision with {collision.gameObject}, probably should not happen !");
            return;
        }

        float healthIncrement = food.type == FoodType.Poison ? -2f : 2f;
        _health = Mathf.Min(_health + healthIncrement, maxHealth);

        Destroy(food.gameObject);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_steeringForce);
    }
}
