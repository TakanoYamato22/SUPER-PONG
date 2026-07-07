using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 7f;
    public float maxSpeed = 25f;
    public float currentSpeed { get; private set; }
    public bool ignoreMaxSpeed = false;

    // ★追加：インスペクターからパーティクルシステムを登録する枠
    public ParticleSystem hitEffect;
    public ParticleSystem smashEffect;

    protected virtual void Start()
    {
        // Ball の初期化処理が必要ならここに書く
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;

        var smash = GetComponent<BallSmashManager>();
        if (smash != null)
            smash.ResetSmash();

        ignoreMaxSpeed = false;
        currentSpeed = baseSpeed;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1f : 1f;

        float y = Random.Range(-0.6f, 0.6f); // 縦方向を弱める

        Vector2 direction = new Vector2(x, y).normalized;

        rb.linearVelocity = direction * baseSpeed;

        currentSpeed = baseSpeed;
    }

    public void IncreaseSpeed(float amount)
    {
        float target = currentSpeed + amount;
        if (ignoreMaxSpeed)
        {
            currentSpeed = target;
        }
        else
        {
            currentSpeed = Mathf.Clamp(target, baseSpeed, maxSpeed);
        }
        rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
    }

    public Vector2 velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    public void SetSpeed(float speed)
    {
        if (ignoreMaxSpeed)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = Mathf.Clamp(speed, baseSpeed, maxSpeed);
        }
        rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
    }

    public void ResetAndStartWithDelay(float delay)
    {
        StartCoroutine(StartAfterDelay(delay));
    }

    private IEnumerator StartAfterDelay(float delay)
    {
        ResetPosition();              // 位置と速度をリセット
        yield return new WaitForSeconds(delay);
        AddStartingForce();           // delay秒後に再スタート
    }

    // ボールが何かに衝突した瞬間に自動で呼ばれる処理
    // Ball.cs の一番下：中身を全部消して、これだけにしてください！
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 中身は空っぽ
    }
}