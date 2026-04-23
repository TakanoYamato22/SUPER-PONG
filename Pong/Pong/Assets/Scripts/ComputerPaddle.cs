using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField]
    private Ball ball;

    public float reactionSpeed = 1.0f; // 1.0 = 即反応, 0.5 = 少し遅い
    public float predictionStrength = 1.0f; // 1.0 = 完全予測, 0.0 = 予測なし

    private void FixedUpdate()
    {
        // --- ボールが右へ向かっている（CPU側へ来ている） ---
        if (ball.velocity.x > 0f)
        {
            // 未来位置予測（Hard / God 用）
            float predictedY = ball.transform.position.y;

            if (predictionStrength > 0f)
            {
                float timeToReach =
                    (transform.position.x - ball.transform.position.x) / ball.velocity.x;

                predictedY = ball.transform.position.y + ball.velocity.y * timeToReach;
            }

            // CPU の反応速度を反映
            float targetY = Mathf.Lerp(transform.position.y, predictedY, reactionSpeed);

            MoveTowards(targetY);
        }
        else
        {
            // --- ボールが離れているときは中央へ戻る ---
            MoveTowards(0f);
        }

        // --- 特殊効果があれば適用 ---
        //activeEffect?.UpdateEffect(this);
    }

    private void MoveTowards(float targetY)
    {
        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        transform.position = new Vector2(transform.position.x, newY);
    }
}