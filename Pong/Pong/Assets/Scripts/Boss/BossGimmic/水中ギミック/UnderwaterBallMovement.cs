
using UnityEngine;

[RequireComponent(typeof(Ball))]
[RequireComponent(typeof(Rigidbody2D))]
public class UnderwaterBallMovement : MonoBehaviour
{
    [Header("水中ギミック")]
    [SerializeField] private bool underwaterEnabled = true;

    [Header("波の設定")]
    [Tooltip("進行方向から左右に曲がる最大角度")]
    [SerializeField, Range(0f, 60f)]
    private float waveStrength = 35f;

    [Tooltip("1秒間に波打つ回数")]
    [SerializeField, Min(0.1f)]
    private float waveFrequency = 1.5f;

    [Header("衝突判定")]
    [SerializeField, Range(10f, 180f)]
    private float reflectionThreshold = 45f;

    private Rigidbody2D rb;

    private float waveTimer;
    private Vector2 baseDirection;
    private Vector2 lastAppliedVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!underwaterEnabled || !rb.simulated)
            return;

        Vector2 currentVelocity = rb.linearVelocity;

        if (currentVelocity.sqrMagnitude < 0.001f)
        {
            ResetWave();
            return;
        }

        float speed = currentVelocity.magnitude;

        // 初回、または反射・スマッシュ後に
        // 新しい基準方向を設定
        if (baseDirection.sqrMagnitude < 0.001f)
        {
            SetBaseDirection(currentVelocity);
        }
        else if (lastAppliedVelocity.sqrMagnitude > 0.001f)
        {
            float angleDifference = Vector2.Angle(
                lastAppliedVelocity,
                currentVelocity
            );

            if (angleDifference >= reflectionThreshold)
            {
                SetBaseDirection(currentVelocity);
            }
        }

        // ========================================
        // 渦潮による引力
        // ========================================

        Vector2 whirlpoolPull =
            Whirlpool.GetCombinedPull(rb.position);

        if (whirlpoolPull.sqrMagnitude > 0.0001f)
        {
            // 速度を変えずに基準方向を曲げる
            baseDirection = (Vector2)Vector3.RotateTowards(
            (Vector3)baseDirection,
            (Vector3)whirlpoolPull.normalized,
            whirlpoolPull.magnitude * Time.fixedDeltaTime,0f).normalized;
        }

        waveTimer += Time.fixedDeltaTime;

        // 基準方向を中心に左右へ蛇行
        float waveAngle = Mathf.Sin(
            waveTimer * waveFrequency * Mathf.PI * 2f
        ) * waveStrength;

        Vector2 newDirection =
            Quaternion.Euler(0f, 0f, waveAngle) *
            baseDirection;

        // 速度を維持
        rb.linearVelocity =
            newDirection.normalized * speed;

        lastAppliedVelocity = rb.linearVelocity;
    }

    private void SetBaseDirection(Vector2 velocity)
    {
        baseDirection = velocity.normalized;
        waveTimer = 0f;
        lastAppliedVelocity = Vector2.zero;
    }

    private void ResetWave()
    {
        waveTimer = 0f;
        baseDirection = Vector2.zero;
        lastAppliedVelocity = Vector2.zero;
    }

    public void SetUnderwaterEnabled(bool enabled)
    {
        underwaterEnabled = enabled;
        ResetWave();
    }
}
