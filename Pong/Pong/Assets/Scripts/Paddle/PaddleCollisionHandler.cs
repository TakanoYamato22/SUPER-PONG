using UnityEngine;

[RequireComponent(typeof(Paddle))]
public class PaddleCollisionHandler : MonoBehaviour
{
    private Paddle paddle;

    private int centerHitCount = 0;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Ball ball = collision.gameObject.GetComponent<Ball>();
        Collider2D col = collision.otherCollider;

        float contactY = (ball.transform.position.y - col.bounds.center.y)
                         / (col.bounds.size.y / 2f);

        float maxBounceAngle = 45f;
        float bounceAngle = contactY * maxBounceAngle;

        float rad = bounceAngle * Mathf.Deg2Rad;
        Vector2 newDir = new Vector2(
            Mathf.Sign(ball.velocity.x) * Mathf.Cos(rad),
            Mathf.Sin(rad)
        ).normalized;

        // ★ まず方向だけ変える（速度は変えない）
        ball.velocity = newDir * ball.currentSpeed;

        // ★ その後に速度を上げる（IncreaseSpeed 内で正しく反映される）
        ball.IncreaseSpeed(1.5f);

        float centerRange = 0.2f;
        if (Mathf.Abs(contactY) < centerRange)
        {
            centerHitCount++;
            PaddleEvents.InvokeCenterHit(paddle, centerHitCount);
        }
        else
        {
            centerHitCount = 0;
        }

        float push = 0.1f;
        ball.transform.position = new Vector2(
            col.bounds.center.x + Mathf.Sign(ball.velocity.x) * (col.bounds.extents.x + push),
            ball.transform.position.y
        );
    }
}
