
using UnityEngine;

public class ComputerPaddle : Paddle
{
    // ========================================
    // Ball設定
    // ========================================

    [Header("Main Ball")]
    [SerializeField]
    private Ball mainBall;

    [Header("Gimmick Balls")]
    [SerializeField]
    private Ball[] gimmicBalls;

    // ========================================
    // FireBall設定
    // ========================================

    [Header("FireBall")]
    [SerializeField]
    private bool targetFireBall = true;

    // ========================================
    // CPU難易度
    // ========================================

    [Header("CPU Difficulty Settings")]
    public float reactionSpeed = 0.6f;
    public float predictionStrength = 0.7f;

    [Header("Human-like Behavior")]
    public float mistakeChance = 0.1f;

    public float limitY = 3.5f;

    // ========================================
    // 移動処理
    // ========================================

    private void FixedUpdate()
    {
        ComputerMove();
    }

    private void ComputerMove()
    {
        if (Random.value < mistakeChance)
        {
            return;
        }

        Vector2 targetPosition;
        Vector2 targetVelocity;

        bool hasTarget = GetDangerousBall(
            out targetPosition,
            out targetVelocity
        );

        float targetY = hasTarget
            ? PredictBallY(targetPosition, targetVelocity)
            : 0f;

        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        newY = Mathf.Clamp(
            newY,
            -limitY,
            limitY
        );

        transform.position = new Vector2(
            transform.position.x,
            newY
        );
    }

    // ========================================
    // 危険なBallを探す
    // ========================================

    private bool GetDangerousBall(
        out Vector2 targetPosition,
        out Vector2 targetVelocity)
    {
        targetPosition = Vector2.zero;
        targetVelocity = Vector2.zero;

        float bestDistance = Mathf.Infinity;
        bool found = false;

        CheckBall(
            mainBall,
            ref targetPosition,
            ref targetVelocity,
            ref bestDistance,
            ref found
        );

        if (gimmicBalls != null)
        {
            foreach (Ball gimmicBall in gimmicBalls)
            {
                CheckBall(
                    gimmicBall,
                    ref targetPosition,
                    ref targetVelocity,
                    ref bestDistance,
                    ref found
                );
            }
        }

        // FireBallを検索
        if (targetFireBall)
        {
            FireSpiritProjectile[] fireBalls =
                FindObjectsByType<FireSpiritProjectile>(
                    FindObjectsSortMode.None
                );

            foreach (FireSpiritProjectile fireBall in fireBalls)
            {
                if (fireBall == null)
                {
                    continue;
                }

                Rigidbody2D fireRb =
                    fireBall.GetComponent<Rigidbody2D>();

                if (fireRb == null)
                {
                    continue;
                }

                CheckTarget(
                    fireBall.transform.position,
                    fireRb.linearVelocity,
                    ref targetPosition,
                    ref targetVelocity,
                    ref bestDistance,
                    ref found
                );
            }
        }

        return found;
    }

    // ========================================
    // 通常Ball確認
    // ========================================

    private void CheckBall(
        Ball ball,
        ref Vector2 targetPosition,
        ref Vector2 targetVelocity,
        ref float bestDistance,
        ref bool found)
    {
        if (ball == null)
        {
            return;
        }

        CheckTarget(
            ball.transform.position,
            ball.velocity,
            ref targetPosition,
            ref targetVelocity,
            ref bestDistance,
            ref found
        );
    }

    // ========================================
    // 共通ターゲット判定
    // ========================================

    private void CheckTarget(
        Vector2 position,
        Vector2 velocity,
        ref Vector2 targetPosition,
        ref Vector2 targetVelocity,
        ref float bestDistance,
        ref bool found)
    {
        // CPU側（右方向）へ飛ぶBallだけ対象
        if (velocity.x <= 0f)
        {
            return;
        }

        float distance = Mathf.Abs(
            transform.position.x - position.x
        );

        if (distance >= bestDistance)
        {
            return;
        }

        bestDistance = distance;
        targetPosition = position;
        targetVelocity = velocity;
        found = true;
    }

    // ========================================
    // 到達位置予測
    // ========================================

    private float PredictBallY(
        Vector2 position,
        Vector2 velocity)
    {
        float predictedY = position.y;

        if (predictionStrength > 0f &&
            Mathf.Abs(velocity.x) > 0.001f)
        {
            float timeToReach =
                (transform.position.x - position.x) /
                velocity.x;

            float futureY =
                position.y +
                velocity.y * timeToReach;

            predictedY = Mathf.Lerp(
                position.y,
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
