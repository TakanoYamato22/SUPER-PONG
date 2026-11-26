
using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(BallSmashManager))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallSmashManager smashManager;

    // ==================================================
    // 速度
    // ==================================================

    [Header("速度")]
    public float baseSpeed = 10f;
    public float maxSpeed = 25f;

    public float currentSpeed
    {
        get;
        private set;
    }

    public bool ignoreMaxSpeed;

    // ==================================================
    // パワーアップ
    // ==================================================

    [Header("パワーアップ")]
    public bool hasPowerUp;
    public float powerMultiplier = 1.5f;

    // ==================================================
    // スマッシュ時のDrone破壊
    // ==================================================

    [Header("スマッシュ時のDrone破壊")]

    [Tooltip("スマッシュ中にDroneを検出する範囲の太さ")]
    [Min(0.01f)]
    [SerializeField]
    private float smashBreakRadius = 0.6f;

    [Tooltip("DroneのLayerだけを選択")]
    [SerializeField]
    private LayerMask droneLayer;

    // ==================================================
    // エフェクト
    // ==================================================

    [Header("エフェクト")]
    public ParticleSystem hitEffect;
    public ParticleSystem smashEffect;

    // ==================================================
    // 初期化
    // ==================================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        smashManager = GetComponent<BallSmashManager>();

        currentSpeed = baseSpeed;
    }

    // ==================================================
    // スマッシュ中のDrone検出
    // ==================================================

    private void FixedUpdate()
    {
        if (smashManager == null)
        {
            return;
        }

        if (!smashManager.IsSmashed)
        {
            return;
        }

        BreakDronesOnSmashPath();
    }

    private void BreakDronesOnSmashPath()
    {
        Vector2 startPosition = rb.position;

        // 次の物理フレームで進む予定の移動量
        Vector2 movement =
            rb.linearVelocity * Time.fixedDeltaTime;

        float distance = movement.magnitude;

        // ボールの進行経路上にあるDroneを検出
        if (distance > 0.001f)
        {
            RaycastHit2D[] hits =
                Physics2D.CircleCastAll(
                    startPosition,
                    smashBreakRadius,
                    movement.normalized,
                    distance + smashBreakRadius,
                    droneLayer
                );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                BreakDrone(hit.collider);
            }
        }

        // 現在位置ですでに重なっているDroneも検出
        Collider2D[] overlaps =
            Physics2D.OverlapCircleAll(
                startPosition,
                smashBreakRadius,
                droneLayer
            );

        foreach (Collider2D overlap in overlaps)
        {
            if (overlap == null)
            {
                continue;
            }

            BreakDrone(overlap);
        }
    }

    private void BreakDrone(Collider2D target)
    {
        if (target == null)
        {
            return;
        }

        Drone drone =
            target.GetComponent<Drone>();

        if (drone == null)
        {
            drone =
                target.GetComponentInParent<Drone>();
        }

        if (drone == null)
        {
            return;
        }

        Debug.Log(
            "スマッシュボールがDroneを破壊：" +
            drone.gameObject.name
        );

        drone.BreakDrone();
    }

    // ==================================================
    // ボールのリセット
    // ==================================================

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;

        if (smashManager != null)
        {
            smashManager.ResetSmash();
        }

        ignoreMaxSpeed = false;
        currentSpeed = baseSpeed;

        hasPowerUp = false;
        powerMultiplier = 1.5f;
    }

    public void AddStartingForce()
    {
        float x =
            Random.value < 0.5f
                ? -1f
                : 1f;

        float y =
            Random.Range(
                -0.6f,
                0.6f
            );

        Vector2 direction =
            new Vector2(x, y).normalized;

        rb.linearVelocity =
            direction * baseSpeed;

        currentSpeed = baseSpeed;
    }

    // ==================================================
    // 速度操作
    // ==================================================

    public void IncreaseSpeed(float amount)
    {
        float targetSpeed =
            currentSpeed + amount;

        if (ignoreMaxSpeed)
        {
            currentSpeed = targetSpeed;
        }
        else
        {
            currentSpeed =
                Mathf.Clamp(
                    targetSpeed,
                    baseSpeed,
                    maxSpeed
                );
        }

        ApplyCurrentSpeed();
    }

    public void SetSpeed(float newSpeed)
    {
        if (ignoreMaxSpeed)
        {
            currentSpeed = newSpeed;
        }
        else
        {
            currentSpeed =
                Mathf.Clamp(
                    newSpeed,
                    baseSpeed,
                    maxSpeed
                );
        }

        ApplyCurrentSpeed();
    }

    private void ApplyCurrentSpeed()
    {
        if (rb.linearVelocity.sqrMagnitude <= 0.001f)
        {
            return;
        }

        rb.linearVelocity =
            rb.linearVelocity.normalized *
            currentSpeed;
    }

    // ==================================================
    // 氷柱による減速（追加）
    // ==================================================

    public void ReduceSpeedByIcicle()
    {
        // 現在速度から5減らす（最低15）
        float newSpeed =
            Mathf.Max(15f, currentSpeed - 5f);

        // 進行方向は変えずに速度だけ減らす
        currentSpeed = newSpeed;
        ApplyCurrentSpeed();

        Debug.Log(
            $"氷柱に衝突！Ball速度：{currentSpeed}"
        );
    }


    public void ReflectByIcicle(Vector2 normal)
    {
        Vector2 direction = rb.linearVelocity.normalized;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        // 氷柱に向かって進んでいる場合だけ反射
        if (Vector2.Dot(direction, normal) < 0f)
        {
            direction = Vector2.Reflect(direction, normal);
        }

        rb.linearVelocity = direction * currentSpeed;
    }


    public Vector2 velocity
    {
        get
        {
            return rb.linearVelocity;
        }

        set
        {
            rb.linearVelocity = value;
        }
    }

    // ==================================================
    // パワーアップ
    // ==================================================

    public void GivePowerUp(float multiplier)
    {
        hasPowerUp = true;
        powerMultiplier = multiplier;
    }

    // ==================================================
    // 遅延リスタート
    // ==================================================

    public void ResetAndStartWithDelay(float delay)
    {
        StartCoroutine(
            StartAfterDelay(delay)
        );
    }

    private IEnumerator StartAfterDelay(float delay)
    {
        ResetPosition();

        yield return new WaitForSeconds(delay);

        AddStartingForce();
    }

    // ==================================================
    // キャラクター倍率
    // ==================================================

    public void ApplyCharacterSpeedMultiplier(
        float multiplier,
        bool allowBelowBaseSpeed
    )
    {
        multiplier =
            Mathf.Max(
                0.01f,
                multiplier
            );

        float targetSpeed =
            currentSpeed *
            multiplier;

        if (allowBelowBaseSpeed)
        {
            currentSpeed =
                Mathf.Clamp(
                    targetSpeed,
                    2f,
                    maxSpeed
                );
        }
        else
        {
            currentSpeed =
                Mathf.Clamp(
                    targetSpeed,
                    baseSpeed,
                    maxSpeed
                );
        }

        if (rb.linearVelocity.sqrMagnitude > 0f)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized *
                currentSpeed;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            smashBreakRadius
        );
    }
#endif
}
