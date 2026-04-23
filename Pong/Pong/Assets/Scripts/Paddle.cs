using UnityEngine;

public abstract class Paddle : MonoBehaviour
{
    public float speed = 5f;
    public bool useDynamicBounce = false;

    // --- 将来の特殊効果用 ---
    //public IPaddleEffect activeEffect;

    public void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Ball ball = collision.gameObject.GetComponent<Ball>();
        Collider2D paddle = collision.otherCollider;

        // パドル中心からの距離（-1〜1）
        float contactY = (ball.transform.position.y - paddle.bounds.center.y)
                         / (paddle.bounds.size.y / 2f);

        float maxBounceAngle = 75f;
        float bounceAngle = contactY * maxBounceAngle;

        // 角度から新しい方向ベクトルを作る
        float rad = bounceAngle * Mathf.Deg2Rad;
        Vector2 newDir = new Vector2(
            Mathf.Sign(ball.velocity.x) * Mathf.Cos(rad),
            Mathf.Sin(rad)
        ).normalized;

        // 加速
        ball.IncreaseSpeed(1.5f);

        // 新しい速度を適用
        ball.velocity = newDir * ball.currentSpeed;
    }


}
