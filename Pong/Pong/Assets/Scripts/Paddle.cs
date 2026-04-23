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

     
        // 🔽 中央ヒット判定（各パドルごと）
        // パドル中心からのズレ（-1〜1）
        float offset = contactY;

        // 中央ヒット範囲（調整可能）
        float centerRange = 0.2f;

        if (Mathf.Abs(offset) < centerRange)
        {
            centerHitCount++;

            // GameManagerに通知（誰がヒットしたかも渡す）
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.OnCenterHit(this, centerHitCount);
            }
        }
        else
        {
            // 中央外したらリセット
            centerHitCount = 0;
        }
    }
    // ===============================
    // 🔽 パドル縮小システム（リスク要素）
    // ===============================
    
    // 🔽 中央ヒットカウント（各パドル専用）
    protected int centerHitCount = 0;
    // 現在縮小中かどうか（連続発動防止）
    private bool isShrinking = false;

    // 外部（GameManagerなど）から呼ばれる関数
    // duration = 何秒間縮むか
    public void Shrink(float duration)
    {
        // すでに縮小中なら何もしない（重複防止）
        if (!isShrinking)
        {
            StartCoroutine(ShrinkCoroutine(duration));
        }
    }

    // 実際の縮小処理（コルーチン）
    private System.Collections.IEnumerator ShrinkCoroutine(float duration)
    {
        // 縮小状態ON
        isShrinking = true;

        // 現在のサイズを保存（元に戻すため）
        Vector3 originalScale = transform.localScale;

        // 🔧 縮小率（ここを調整すれば難易度変わる）
        float shrinkMultiplier = 0.6f;

        // 🔴 高さ（Y）だけ縮める ←重要ポイント
        transform.localScale = new Vector3(
            originalScale.x,                      // 横幅はそのまま
            originalScale.y * shrinkMultiplier,   // 高さだけ縮小
            originalScale.z
        );

        // 指定時間待つ
        yield return new WaitForSeconds(duration);

        // 元のサイズに戻す
        transform.localScale = originalScale;

        // 縮小状態OFF
        isShrinking = false;
    }

}
