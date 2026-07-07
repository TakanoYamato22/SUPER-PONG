using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 7f;
    public float maxSpeed = 25f;
    public float currentSpeed { get; private set; }

    public bool hasPowerUp = false;
    public float powerMultiplier = 1.5f;

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

        hasPowerUp = false;
        powerMultiplier = 1.5f;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.Range(-0.6f, 0.6f); // 🌟ここにあった重複宣言の不具合を解消しました

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

    public void GivePowerUp(float multiplier)
    {
        hasPowerUp = true;
        powerMultiplier = multiplier;
    }

    public void ResetAndStartWithDelay(float delay)
    {
        StartCoroutine(StartAfterDelay(delay));
    }

    private IEnumerator StartAfterDelay(float delay)
    {
        ResetPosition();
        yield return new WaitForSeconds(delay);
        AddStartingForce();
    }

    // ボールが何かに衝突した瞬間に自動で呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallSmashManager smash = GetComponent<BallSmashManager>();

        if (smash == null || !smash.IsSmashed) return;

        if (collision.CompareTag("Drone"))
        {
            Destroy(collision.gameObject);
        }
    
    }

}