
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class WaterSpiritShield : MonoBehaviour
{
    // ========================================
    // シールド設定
    // ========================================

    [Header("水の精霊")]
    [SerializeField]
    private ForestSpirit waterSpirit;

    [Header("シールド中心")]
    [SerializeField]
    private Transform shieldCenter;

    [Header("シールドのCollider")]
    [SerializeField]
    private CircleCollider2D shieldCollider;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        if (shieldCenter == null)
        {
            shieldCenter = transform;
        }

        if (shieldCollider == null)
        {
            shieldCollider = GetComponent<CircleCollider2D>();
        }

        shieldCollider.isTrigger = true;

        if (waterSpirit == null)
        {
            waterSpirit =
                GetComponentInParent<ForestSpirit>();
        }
    }

    // ========================================
    // Ballとの衝突
    // ========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();

        if (ball == null)
        {
            return;
        }

        // 精霊が倒されていたら処理しない
        if (waterSpirit != null && waterSpirit.IsDead)
        {
            return;
        }

        BallSmashManager smashManager =
            ball.GetComponent<BallSmashManager>();

        // ========================================
        // Smash中はシールドを貫通
        // ========================================

        if (smashManager != null &&
            smashManager.IsSmashed)
        {
            Debug.Log(
                "Smash Ballが水のシールドを貫通！",
                this
            );

            return;
        }

        // ========================================
        // 通常Ballを反射
        // ========================================

        ReflectBall(ball);
    }

    // ========================================
    // 円形シールドの反射処理
    // ========================================

    private void ReflectBall(Ball ball)
    {
        Vector2 ballPosition =
            ball.transform.position;

        Vector2 centerPosition =
            shieldCenter.position;

        // 中心からBallへの方向を計算
        Vector2 normal =
            ballPosition - centerPosition;

        if (normal.sqrMagnitude < 0.0001f)
        {
            normal = -ball.velocity.normalized;
        }

        if (normal.sqrMagnitude < 0.0001f)
        {
            return;
        }

        normal.Normalize();

        // Ballの既存の反射処理を使用
        ball.ReflectByIcicle(normal);

        Debug.Log(
            "水の精霊のシールドがBallを反射！",
            this
        );
    }
}
