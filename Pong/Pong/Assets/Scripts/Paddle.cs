using UnityEngine;

public abstract class Paddle : MonoBehaviour
{
    public float speed = 5f;

    protected int centerHitCount = 0;
    private bool isShrinking = false;

    public void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Ball ball = collision.gameObject.GetComponent<Ball>();
        Collider2D paddle = collision.otherCollider;

        float contactY = (ball.transform.position.y - paddle.bounds.center.y)
                         / (paddle.bounds.size.y / 2f);

        float maxBounceAngle = 75f;
        float bounceAngle = contactY * maxBounceAngle;

        float rad = bounceAngle * Mathf.Deg2Rad;
        Vector2 newDir = new Vector2(
            Mathf.Sign(ball.velocity.x) * Mathf.Cos(rad),
            Mathf.Sin(rad)
        ).normalized;

        // プレイヤーだけ加速
        if (this is PlayerPaddle)
        {
            ball.IncreaseSpeed(1.5f);
        }

        ball.velocity = newDir * ball.currentSpeed;

        // 中央ヒット判定
        float centerRange = 0.2f;

        if (Mathf.Abs(contactY) < centerRange)
        {
            centerHitCount++;
            PaddleEvents.InvokeCenterHit(this, centerHitCount);
        }
        else
        {
            centerHitCount = 0;
        }
    }

    public void Shrink(float duration)
    {
        if (!isShrinking)
        {
            StartCoroutine(ShrinkCoroutine(duration));
        }
    }

    private System.Collections.IEnumerator ShrinkCoroutine(float duration)
    {
        isShrinking = true;

        Vector3 originalScale = transform.localScale;
        float shrinkMultiplier = 0.6f;

        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * shrinkMultiplier,
            originalScale.z
        );

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
        isShrinking = false;
    }
}
