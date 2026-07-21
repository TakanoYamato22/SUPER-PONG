using UnityEngine;

public class GimmickBall : MonoBehaviour
{
    [SerializeField] private float keepSpeed = 7f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * keepSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Paddle")) return;

        Vector2 velocity = rb.linearVelocity;
        velocity.x *= -1f;

        rb.linearVelocity =
            velocity.normalized * keepSpeed;
    }
}