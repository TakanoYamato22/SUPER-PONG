using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 10f;
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

<<<<<<< HEAD
<<<<<<< HEAD
    private void Update()
    {
        BreakDroneWhileSmashing();
    }

    private void BreakDroneWhileSmashing()
    {
        BallSmashManager smash = GetComponent<BallSmashManager>();

        if (smash == null || !smash.IsSmashed)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, smashBreakRadius);

        foreach (Collider2D hit in hits)
        {
            Drone drone = hit.GetComponent<Drone>();

            if (drone != null)
            {
                drone.BreakDrone();
                continue;
            }

            FixedDrone fixedDrone = hit.GetComponent<FixedDrone>();

            if (fixedDrone != null)
            {
                Destroy(fixedDrone.gameObject);
            }
        }
    }

=======
>>>>>>> parent of 094f2fa (Merge pull request #42 from TakanoYamato22/sota2)
=======
>>>>>>> parent of 0914100 (i)
    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;

        BallSmashManager smash = GetComponent<BallSmashManager>();
        if (smash != null)
        {
            smash.ResetSmash();
        }

        ignoreMaxSpeed = false;
        currentSpeed = baseSpeed;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
<<<<<<< HEAD
<<<<<<< HEAD
        float y = Random.Range(-0.6f, 0.6f);
=======

        float y = Random.Range(-0.6f, 0.6f); // 縦方向を弱める
>>>>>>> parent of 094f2fa (Merge pull request #42 from TakanoYamato22/sota2)
=======
        float y = Random.Range(-0.6f, 0.6f); // 🌟ここにあった重複宣言の不具合を解消しました
>>>>>>> parent of 0914100 (i)

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
<<<<<<< HEAD
<<<<<<< HEAD

=======
>>>>>>> parent of 094f2fa (Merge pull request #42 from TakanoYamato22/sota2)
=======
>>>>>>> parent of 0914100 (i)
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
<<<<<<< HEAD
<<<<<<< HEAD

=======
>>>>>>> parent of 094f2fa (Merge pull request #42 from TakanoYamato22/sota2)
=======
>>>>>>> parent of 0914100 (i)
        rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
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
<<<<<<< HEAD
<<<<<<< HEAD
=======

    // ボールが何かに衝突した瞬間に自動で呼ばれる処理
    // Ball.cs の一番下：中身を全部消して、これだけにしてください！
    private void OnCollisionEnter2D(Collision2D collision)
=======

    // ボールが何かに衝突した瞬間に自動で呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
>>>>>>> parent of 0914100 (i)
    {
        BallSmashManager smash = GetComponent<BallSmashManager>();

        if (smash == null || !smash.IsSmashed) return;

        if (collision.CompareTag("Drone"))
        {
            Destroy(collision.gameObject);
        }
    
    }

<<<<<<< HEAD
>>>>>>> parent of 094f2fa (Merge pull request #42 from TakanoYamato22/sota2)
=======
>>>>>>> parent of 0914100 (i)
}