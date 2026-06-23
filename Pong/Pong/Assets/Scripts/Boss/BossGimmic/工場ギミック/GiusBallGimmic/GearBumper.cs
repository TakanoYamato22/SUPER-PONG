using UnityEngine;

public class GearBumper : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 15f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();

        if (ball == null)
            return;

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        if (rb == null)
            return;

        // ランダム方向
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 真上・真下になりすぎ防止
        if (Mathf.Abs(randomDirection.x) < 0.3f)
        {
            randomDirection.x = randomDirection.x >= 0 ? 0.3f : -0.3f;
            randomDirection.Normalize();
        }

        rb.linearVelocity = randomDirection * launchSpeed;
    }
}