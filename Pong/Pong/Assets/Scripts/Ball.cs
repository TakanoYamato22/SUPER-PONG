using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 7f;
    public float maxSpeed = 25f;
    public float currentSpeed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(x, y).normalized;

        rb.AddForce(direction * baseSpeed, ForceMode2D.Impulse);
        currentSpeed = baseSpeed;
    }

    public void IncreaseSpeed(float amount)
    {
        currentSpeed = Mathf.Min(currentSpeed + amount, maxSpeed);
        rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
    }

    public Vector2 velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁ダメージだけ Ball が担当
        if (collision.gameObject.TryGetComponent(out WallBlock wall))
        {
            wall.TakeDamage();
        }
    }
}
