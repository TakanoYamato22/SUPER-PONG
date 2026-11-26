
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CircusFakeBall : MonoBehaviour
{
    // ========================================
    // 移動設定
    // ========================================

    [Header("移動設定")]
    [SerializeField, Min(0.1f)]
    private float moveSpeed = 8f;

    [SerializeField, Min(0f)]
    private float steeringSpeed = 90f;

    [Header("反射設定")]
    [Tooltip("反射後、追尾を停止する時間")]
    [SerializeField, Min(0f)]
    private float reflectionDelay = 0.5f;

    [Header("Paddle")]
    [SerializeField]
    private Transform player1Paddle;

    [SerializeField]
    private Transform player2Paddle;

    // ========================================
    // ダメージ・生存時間
    // ========================================

    [Header("ダメージ設定")]
    [SerializeField, Min(0f)]
    private float goalDamage = 15f;

    [Header("生存時間")]
    [SerializeField, Min(0.1f)]
    private float lifeTime = 10f;

    // ========================================
    // 内部変数
    // ========================================

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private Vector2 moveDirection;
    private Transform targetPaddle;

    private float lifeTimer;
    private float reflectionTimer;

    private bool initialized;
    private bool disappearing;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        circleCollider.isTrigger = false;
    }

    public void Initialize(
        Transform paddle1,
        Transform paddle2
    )
    {
        player1Paddle = paddle1;
        player2Paddle = paddle2;

        ChooseTarget();

        if (targetPaddle == null)
        {
            Debug.LogWarning(
                "【Circus】FakeBallの追尾対象がありません！",
                this
            );

            Destroy(gameObject);
            return;
        }

        Vector2 direction =
            (Vector2)targetPaddle.position - rb.position;

        moveDirection = direction.normalized;

        if (moveDirection.sqrMagnitude < 0.001f)
        {
            moveDirection = Vector2.right;
        }

        lifeTimer = lifeTime;
        reflectionTimer = 0f;

        initialized = true;
        disappearing = false;

        rb.linearVelocity = moveDirection * moveSpeed;

        Debug.Log(
            $"【Circus】FakeBall出現！追尾対象：{targetPaddle.name}"
        );
    }

    // ========================================
    // 追尾対象の決定
    // ========================================

    private void ChooseTarget()
    {
        if (player1Paddle != null &&
            player2Paddle != null)
        {
            targetPaddle = Random.value < 0.5f
                ? player1Paddle
                : player2Paddle;
        }
        else if (player1Paddle != null)
        {
            targetPaddle = player1Paddle;
        }
        else
        {
            targetPaddle = player2Paddle;
        }
    }

    // ========================================
    // 移動・追尾
    // ========================================

    private void FixedUpdate()
    {
        if (!initialized || disappearing)
        {
            return;
        }

        // 反射後は一定時間、追尾しない
        if (reflectionTimer > 0f)
        {
            reflectionTimer -= Time.fixedDeltaTime;

            rb.linearVelocity = moveDirection * moveSpeed;
            return;
        }

        if (targetPaddle == null)
        {
            return;
        }

        Vector2 targetDirection =
            ((Vector2)targetPaddle.position -
             rb.position).normalized;

        if (targetDirection.sqrMagnitude < 0.001f)
        {
            return;
        }

        float maxRadians =
            steeringSpeed *
            Mathf.Deg2Rad *
            Time.fixedDeltaTime;

        Vector3 rotatedDirection =
            Vector3.RotateTowards(
                moveDirection,
                targetDirection,
                maxRadians,
                0f
            );

        moveDirection =
            ((Vector2)rotatedDirection).normalized;

        rb.linearVelocity =
            moveDirection * moveSpeed;
    }

    // ========================================
    // 壁・Paddle反射
    // ========================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!initialized || disappearing)
        {
            return;
        }

        if (collision.contactCount == 0)
        {
            return;
        }

        // 衝突した面の法線
        Vector2 normal =
            collision.GetContact(0).normal;

        // 現在の進行方向を反射
        Vector2 reflectedDirection =
            Vector2.Reflect(moveDirection, normal);

        if (reflectedDirection.sqrMagnitude < 0.001f)
        {
            reflectedDirection = -moveDirection;
        }

        moveDirection = reflectedDirection.normalized;

        rb.linearVelocity =
            moveDirection * moveSpeed;

        // 反射後は一定時間追尾を停止
        reflectionTimer = reflectionDelay;

        Debug.Log(
            $"【Circus】FakeBall反射：{collision.gameObject.name}"
        );
    }

    // ========================================
    // 生存時間
    // ========================================

    private void Update()
    {
        if (!initialized || disappearing)
        {
            return;
        }

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            Disappear();
        }
    }

    // ========================================
    // Goal判定
    // ========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!initialized || disappearing)
        {
            return;
        }

        if (!other.CompareTag("Goal"))
        {
            return;
        }

        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health == null)
        {
            health =
                other.GetComponentInParent<PlayerHealth>();
        }

        if (health == null)
        {
            health =
                other.GetComponentInChildren<PlayerHealth>();
        }

        if (health != null)
        {
            health.TakeDamage(goalDamage);

            Debug.Log(
                $"【Circus】FakeBallがGoalに到達！" +
                $" {health.name}に{goalDamage}ダメージ！"
            );
        }
        else
        {
            Debug.LogWarning(
                "【Circus】GoalにPlayerHealthが見つかりません！",
                other
            );
        }

        Disappear();
    }

    // ========================================
    // 消滅
    // ========================================

    private void Disappear()
    {
        if (disappearing)
        {
            return;
        }

        disappearing = true;
        initialized = false;

        rb.linearVelocity = Vector2.zero;

        Destroy(gameObject);
    }
}
