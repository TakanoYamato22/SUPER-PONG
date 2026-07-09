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

    public float limitY = 3.5f;

    private void FixedUpdate()
    {
        ComputerMove();
    }

    private void ComputerMove()
    {
        // たまにミスする（人間っぽさ）
        if (Random.value < mistakeChance)
            return;

        float targetY;

        // ボールがCPU側に向かっているときだけ追う
        if (ball.velocity.x > 0f)
        {
            float predictedY = ball.transform.position.y;

            // 未来予測（強いCPUほど予測精度が高い）
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

            // 反応速度（強いCPUほど素早く追う）
            targetY = Mathf.Lerp(
                transform.position.y,
                predictedY,
                reactionSpeed
            );
        }
        else
        {
            // ボールが離れたら中央に戻る
            targetY = 0f;
        }

        // プレイヤーと同じ MoveTowards + Clamp
        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        newY = Mathf.Clamp(newY, -limitY, limitY);

        transform.position = new Vector2(transform.position.x, newY);
    }
}