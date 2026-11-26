
using System.Collections.Generic;
using UnityEngine;

public class WaterShockwave : MonoBehaviour
{
    [Header("衝撃波の拡大")]
    [SerializeField, Min(0.01f)]
    private float startRadius = 0.2f;

    [SerializeField, Min(0.1f)]
    private float maxRadius = 12f;

    [SerializeField, Min(0.1f)]
    private float expandDuration = 1.2f;

    [Header("当たり判定")]
    [SerializeField, Min(0.01f)]
    private float ringThickness = 0.5f;

    private float elapsedTime;
    private float previousRadius;
    private float currentRadius;

    private Vector3 originalScale;

    private readonly HashSet<Ball> hitBalls =
        new HashSet<Ball>();

    private void Awake()
    {
        originalScale = transform.localScale;
        previousRadius = startRadius;
        currentRadius = startRadius;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float progress = Mathf.Clamp01(
            elapsedTime / expandDuration
        );

        previousRadius = currentRadius;

        currentRadius = Mathf.Lerp(
            startRadius,
            maxRadius,
            progress
        );

        // 画像の直径が1 Unity Unitの場合の拡大率
        float diameter = currentRadius * 2f;

        transform.localScale =
            originalScale * diameter;

        CheckBallHit();

        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void CheckBallHit()
    {
        Ball[] balls = FindObjectsByType<Ball>(
            FindObjectsSortMode.None
        );

        foreach (Ball ball in balls)
        {
            if (ball == null || hitBalls.Contains(ball))
                continue;

            Rigidbody2D rb =
                ball.GetComponent<Rigidbody2D>();

            if (rb == null || !rb.simulated)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                ball.transform.position
            );

            // 拡大する輪が通過した範囲を判定
            float innerRadius = Mathf.Max(
                0f,
                previousRadius - ringThickness * 0.5f
            );

            float outerRadius =
                currentRadius + ringThickness * 0.5f;

            if (distance < innerRadius ||
                distance > outerRadius)
            {
                continue;
            }

            hitBalls.Add(ball);
            ApplyShockwave(ball);
        }
    }

    private void ApplyShockwave(Ball ball)
    {
        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;

        if (randomDirection.sqrMagnitude < 0.001f)
            randomDirection = Vector2.right;

        BallSmashManager smash =
            ball.GetComponent<BallSmashManager>();

        // スマッシュ状態は変更しない
        // 最高速度へ設定して進行方向を変更する
        ball.SetSpeed(ball.maxSpeed);

        ball.velocity =
            randomDirection * ball.currentSpeed;

        Debug.Log(
            $"衝撃波命中！速度：{ball.currentSpeed}" +
            $" / スマッシュ中：{(smash != null && smash.IsSmashed)}"
        );
    }
}
