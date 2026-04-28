using UnityEngine;

public abstract class Paddle : MonoBehaviour
{
    public float speed = 5f;

    // 中央ヒットカウント
    protected int centerHitCount = 0;

    // 縮小中フラグ
    private bool isShrinking = false;


    // -------------------------
    // パドル位置リセット
    // -------------------------
    public void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, 0f);
    }


    // -------------------------
    // ボール衝突処理（縦カン防止版）
    // -------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Ball ball = collision.gameObject.GetComponent<Ball>();
        Collider2D paddle = collision.otherCollider;

        // パドル中心からの距離（-1〜1）
        float contactY = (ball.transform.position.y - paddle.bounds.center.y)
                         / (paddle.bounds.size.y * 0.5f);

        // ★ 縦カン防止：上下端の反射角を弱める
        contactY = Mathf.Clamp(contactY, -0.8f, 0.8f);

        // 最大反射角
        float maxBounceAngle = 75f;
        float bounceAngle = contactY * maxBounceAngle;

        // 角度 → 方向ベクトル
        float rad = bounceAngle * Mathf.Deg2Rad;
        Vector2 newDir = new Vector2(
            Mathf.Sign(ball.velocity.x) * Mathf.Cos(rad),
            Mathf.Sin(rad)
        ).normalized;

        // 加速
        ball.IncreaseSpeed(1.5f);

        // 新しい速度を適用
        ball.velocity = newDir * ball.currentSpeed;


        // -------------------------
        // 中央ヒット判定
        // -------------------------
        float centerRange = 0.2f;

        if (Mathf.Abs(contactY) < centerRange)
        {
            centerHitCount++;

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.OnCenterHit(this, centerHitCount);
            }
        }
        else
        {
            centerHitCount = 0;
        }
    }


    // -------------------------
    // パドル縮小（アイテム効果など）
    // -------------------------
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

        // 縮小率
        float shrinkMultiplier = 0.6f;

        // 高さだけ縮める
        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * shrinkMultiplier,
            originalScale.z
        );

        yield return new WaitForSeconds(duration);

        // 元に戻す
        transform.localScale = originalScale;

        isShrinking = false;
    }
}
