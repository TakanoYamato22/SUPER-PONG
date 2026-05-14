using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField]
    private Ball ball;

    [Header("CPU Difficulty Settings")]
    public float reactionSpeed = 0.6f;
    public float predictionStrength = 0.7f;

    [Header("Human-like Behavior")]
    public float mistakeChance = 0.1f;

    public float limitY = 3.5f; // ← Player と同じ上下制限

    private void FixedUpdate()
    {
        if (Random.value < mistakeChance)
            return;

        float targetY;

        // ボールがCPU側に向かっているとき
        if (ball.velocity.x > 0f)
        {
            float predictedY = ball.transform.position.y;

            if (predictionStrength > 0f)
            {
                float timeToReach =
                    (transform.position.x - ball.transform.position.x) / ball.velocity.x;

                float futureY =
                    ball.transform.position.y + ball.velocity.y * timeToReach;

                predictedY = Mathf.Lerp(
                    ball.transform.position.y,
                    futureY,
                    predictionStrength
                );
            }

            targetY = Mathf.Lerp(
                transform.position.y,
                predictedY,
                reactionSpeed
            );
        }
        else
        {
            targetY = 0f; // 中央に戻る
        }

        // ★ Player と同じ移動方式に統一
        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        // ★ Player と同じ上下制限
        newY = Mathf.Clamp(newY, -limitY, limitY);

        transform.position = new Vector2(transform.position.x, newY);
    }
}
