
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class FireSpiritProjectile : MonoBehaviour
{
    // ========================================
    // ダメージ
    // ========================================

    [Header("ダメージ")]
    [SerializeField, Min(0f)]
    private float reflectedDamage = 20f;

    [SerializeField, Min(0f)]
    private float goalDamage = 15f;

    // ========================================
    // 波の設定
    // ========================================

    [Header("波の設定")]
    [SerializeField, Range(0f, 60f)]
    private float waveStrength = 35f;

    [SerializeField, Min(0.1f)]
    private float waveFrequency = 1.5f;

    // ========================================
    // 反射設定
    // ========================================

    [Header("反射設定")]
    [SerializeField, Range(10f, 180f)]
    private float reflectionThreshold = 45f;

    [SerializeField, Min(0f)]
    private float pushDistance = 0.1f;

    // ========================================
    // 寿命
    // ========================================

    [Header("寿命")]
    [SerializeField, Min(0.1f)]
    private float lifeTime = 15f;

    // ========================================
    // 内部変数
    // ========================================

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private ForestSpirit ownerSpirit;

    private Vector2 baseDirection;
    private Vector2 lastAppliedVelocity;

    private float speed;
    private float waveTimer;

    private bool isReflected;
    private bool initialized;
    private bool isFinished;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        rb.gravityScale = 0f;
        circleCollider.isTrigger = false;
    }

    public void Initialize(
        Vector2 direction,
        float shootSpeed,
        ForestSpirit owner)
    {
        ownerSpirit = owner;

        speed = Mathf.Max(0.1f, shootSpeed);

        baseDirection = direction.normalized;

        if (baseDirection.sqrMagnitude < 0.001f)
        {
            baseDirection = Vector2.left;
        }

        waveTimer = 0f;
        lastAppliedVelocity = Vector2.zero;

        isReflected = false;
        isFinished = false;
        initialized = true;

        rb.linearVelocity = baseDirection * speed;

        Destroy(gameObject, lifeTime);
    }

    // ========================================
    // 波状移動
    // ========================================

    private void FixedUpdate()
    {
        if (!initialized || isFinished || !rb.simulated)
        {
            return;
        }

        Vector2 currentVelocity = rb.linearVelocity;

        if (currentVelocity.sqrMagnitude < 0.001f)
        {
            rb.linearVelocity = baseDirection * speed;
            return;
        }

        // 壁などで反射した方向を検出
        if (lastAppliedVelocity.sqrMagnitude > 0.001f)
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

        waveTimer += Time.fixedDeltaTime;

        float waveAngle = Mathf.Sin(
            waveTimer *
            waveFrequency *
            Mathf.PI *
            2f
        ) * waveStrength;

        Vector2 newDirection =
            Quaternion.Euler(0f, 0f, waveAngle) *
            baseDirection;

        // 速度は常に一定
        rb.linearVelocity =
            newDirection.normalized * speed;

        lastAppliedVelocity = rb.linearVelocity;
    }

    // ========================================
    // 基準方向変更
    // ========================================

    private void SetBaseDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        baseDirection = direction.normalized;
        waveTimer = 0f;
        lastAppliedVelocity = Vector2.zero;

        rb.linearVelocity = baseDirection * speed;
    }

    // ========================================
    // パドル専用反射
    // ========================================

    public void ReflectByPaddle(
        Collider2D paddleCollider,
        float maxBounceAngle)
    {
        if (!initialized || isFinished ||
            paddleCollider == null)
        {
            return;
        }

        Bounds bounds = paddleCollider.bounds;

        // パドルの中心からの接触位置
        float halfHeight = Mathf.Max(
            bounds.extents.y,
            0.01f
        );

        float contactY =
            (transform.position.y - bounds.center.y) /
            halfHeight;

        contactY = Mathf.Clamp(contactY, -1f, 1f);

        // 接触位置から反射角度を計算
        float bounceAngle =
            contactY * maxBounceAngle;

        float radians =
            bounceAngle * Mathf.Deg2Rad;

        // パドルの左右どちらへ跳ね返すか
        float horizontalDirection =
            transform.position.x >= bounds.center.x
                ? 1f
                : -1f;

        Vector2 newDirection = new Vector2(
            horizontalDirection * Mathf.Cos(radians),
            Mathf.Sin(radians)
        ).normalized;

        // 波の基準方向を更新
        SetBaseDirection(newDirection);

        isReflected = true;

        // パドルの外側へ押し出す
        float ballRadius =
            circleCollider.bounds.extents.x;

        Vector2 newPosition = rb.position;

        newPosition.x =
            bounds.center.x +
            horizontalDirection *
            (bounds.extents.x + ballRadius + pushDistance);

        rb.position = newPosition;

        Debug.Log("FireBallがパドルで反射！");
    }

    // ========================================
    // 精霊への接触
    // ========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!initialized || isFinished)
        {
            return;
        }

        ForestSpirit spirit =
            other.GetComponentInParent<ForestSpirit>();

        if (spirit == null)
        {
            return;
        }

        if (spirit == ownerSpirit && isReflected)
        {
            isFinished = true;

            spirit.TakeDamage(reflectedDamage);

            Destroy(gameObject);
        }
    }

    // ========================================
    // Goal到達
    // ========================================

    public void HitGoal(PlayerHealth playerHealth)
    {
        if (isFinished)
        {
            return;
        }

        isFinished = true;

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(goalDamage);
        }

        Destroy(gameObject);
    }
}
