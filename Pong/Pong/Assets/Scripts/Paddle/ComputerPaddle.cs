using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField] private Ball mainBall;

    [Header("Gimmic Ball")]
    [SerializeField] private Ball[] gimmicBalls;

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

        Ball targetBall = GetDangerousBall();

        float targetY;

        if (targetBall != null)
        {
            targetY = PredictBallY(targetBall);
        }
        else
        {
            targetY = 0f;
        }

        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        newY = Mathf.Clamp(newY, -limitY, limitY);

        transform.position = new Vector2(transform.position.x, newY);
    }

    private Ball GetDangerousBall()
    {
        Ball bestBall = null;
        float bestDistance = Mathf.Infinity;

        CheckBall(mainBall, ref bestBall, ref bestDistance);

        if (gimmicBalls != null)
        {
            foreach (Ball gimmicBall in gimmicBalls)
            {
                CheckBall(gimmicBall, ref bestBall, ref bestDistance);
            }
        }

        return bestBall;
    }

    private void CheckBall(Ball ball, ref Ball bestBall, ref float bestDistance)
    {
        if (ball == null) return;

        // CPU側、つまり右に向かっているBallだけ見る
        if (ball.velocity.x <= 0f) return;

        float distance = Mathf.Abs(transform.position.x - ball.transform.position.x);

        if (distance < bestDistance)
        {
            bestDistance = distance;
            bestBall = ball;
        }
    }

    private float PredictBallY(Ball ball)
    {
        float predictedY = ball.transform.position.y;

        if (predictionStrength > 0f && ball.velocity.x != 0f)
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

        return Mathf.Lerp(
            transform.position.y,
            predictedY,
            reactionSpeed
        );
    }
}