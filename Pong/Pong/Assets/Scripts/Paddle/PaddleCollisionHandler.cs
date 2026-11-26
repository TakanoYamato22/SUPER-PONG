
using UnityEngine;

[RequireComponent(typeof(Paddle))]
public class PaddleCollisionHandler : MonoBehaviour
{
    // ========================================
    // 通常反射
    // ========================================

    [Header("通常反射時の基本加速量")]
    [SerializeField]
    private float baseSpeedIncrease = 1.5f;

    [Header("反射角度")]
    [SerializeField]
    private float maxBounceAngle = 45f;

    // ========================================
    // 参照
    // ========================================

    private CharacterRuntimeStats characterStats;

    private void Awake()
    {
        characterStats =
            GetComponent<CharacterRuntimeStats>();
    }

    // ========================================
    // 衝突処理
    // ========================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ====================================
        // FireBallの反射
        // ====================================

        FireSpiritProjectile fireBall =
            collision.gameObject
                .GetComponent<FireSpiritProjectile>();

        if (fireBall != null)
        {
            fireBall.ReflectByPaddle(
                collision.otherCollider,
                maxBounceAngle
            );

            return;
        }

        // ====================================
        // 通常Ball以外は無視
        // ====================================

        if (!collision.gameObject.CompareTag("Ball"))
        {
            return;
        }

        Ball ball =
            collision.gameObject.GetComponent<Ball>();

        if (ball == null)
        {
            return;
        }

        Collider2D paddleCollider =
            collision.otherCollider;

        float contactY =
            (ball.transform.position.y -
             paddleCollider.bounds.center.y) /
            (paddleCollider.bounds.size.y / 2f);

        contactY = Mathf.Clamp(
            contactY,
            -1f,
            1f
        );

        // ====================================
        // 反射角度
        // ====================================

        float bounceAngle =
            contactY * maxBounceAngle;

        float radians =
            bounceAngle * Mathf.Deg2Rad;

        float horizontalDirection;

        if (transform.position.x <
            ball.transform.position.x)
        {
            horizontalDirection = 1f;
        }
        else
        {
            horizontalDirection = -1f;
        }

        Vector2 newDirection = new Vector2(
            horizontalDirection * Mathf.Cos(radians),
            Mathf.Sin(radians)
        ).normalized;

        // ====================================
        // 通常Ballの速度処理
        // ====================================

        ball.velocity =
            newDirection * ball.currentSpeed;

        ball.IncreaseSpeed(baseSpeedIncrease);

        float characterMultiplier = 1f;

        if (characterStats != null)
        {
            characterMultiplier =
                characterStats.NormalHitSpeedMultiplier;
        }

        if (!Mathf.Approximately(
            characterMultiplier,
            1f))
        {
            ball.ApplyCharacterSpeedMultiplier(
                characterMultiplier,
                false
            );
        }

        // ====================================
        // パドル外へ押し出す
        // ====================================

        float pushDistance = 0.1f;

        ball.transform.position = new Vector2(
            paddleCollider.bounds.center.x +
            horizontalDirection *
            (paddleCollider.bounds.extents.x +
             pushDistance),

            ball.transform.position.y
        );
    }
}
