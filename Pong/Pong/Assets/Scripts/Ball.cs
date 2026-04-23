using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 7f;     // 初速
    public float maxSpeed = 25f;     // 加速の上限
    public float currentSpeed { get; set; }

    [SerializeField] private float centerRange = 0.2f;
    private int centerHitCount = 0;

    private GameManager gm;

    // パドルに当たるたびに加速
    public void IncreaseSpeed(float amount)
    {
        currentSpeed = Mathf.Min(currentSpeed + amount, maxSpeed);
        velocity = velocity.normalized * currentSpeed;
    }

    // velocity プロパティ（Rigidbody2D の linearVelocity をラップ）
    public Vector2 velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.value < 0.5f ? Random.Range(-1f, -0.5f)
                                      : Random.Range(0.5f, 1f);

        Vector2 direction = new Vector2(x, y).normalized;

        rb.AddForce(direction * baseSpeed, ForceMode2D.Impulse);
        currentSpeed = baseSpeed;
    }

    private void FixedUpdate()
    {
        // 速度が0にならないようにだけ補正
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            Vector2 dir = rb.linearVelocity.normalized;
            rb.linearVelocity = dir * currentSpeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Paddle")) return;

        Collider2D paddle = collision.collider;

        // パドル中心からどれくらいズレてるか（-1〜1）
        float offset = (transform.position.y - paddle.bounds.center.y)
                       / (paddle.bounds.size.y / 2f);

        // ============================
        // 🧪 デバッグ用ログ
        // ============================

        Debug.Log("offset: " + offset);

        // ============================
        // 🎯 中央ヒット判定（ログだけ）
        // ============================

        if (Mathf.Abs(offset) < centerRange)
        {
            Debug.Log("🔥 中央ヒット！（Ball側ログ）");
        }
        else
        {
            Debug.Log("通常ヒット（Ball側ログ）");
        }

        // ============================
        // 🧱 壁ダメージ
        // ============================

        if (collision.gameObject.TryGetComponent(out WallBlock wall))
        {
            wall.TakeDamage();
        }
    }
    }
