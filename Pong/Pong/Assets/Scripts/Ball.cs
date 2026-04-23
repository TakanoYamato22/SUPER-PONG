using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 7f;     // 初速
    public float maxSpeed = 25f;     // 加速の上限
    public float currentSpeed { get; set; }

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

}
