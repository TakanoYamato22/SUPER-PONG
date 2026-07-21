using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BallSmashManager))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallSmashManager smashManager;

    [Header("速度")]
    public float baseSpeed = 10f;
    public float maxSpeed = 25f;

    public float currentSpeed
    {
        get;
        private set;
    }

    public bool ignoreMaxSpeed;

    [Header("パワーアップ")]
    public bool hasPowerUp;
    public float powerMultiplier = 1.5f;

    [Header("スマッシュ時のDrone破壊")]
    [Tooltip("スマッシュ中にDroneを検出する太さ")]
    [Min(0.01f)]
    [SerializeField]
    private float smashBreakRadius = 0.35f;

    [Tooltip("Drone Layerだけを選択")]
    [SerializeField]
    private LayerMask droneLayer;

    [Header("エフェクト")]
    public ParticleSystem hitEffect;
    public ParticleSystem smashEffect;

    private Vector2 previousPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        smashManager =
            GetComponent<BallSmashManager>();

        currentSpeed = baseSpeed;
        previousPosition = rb.position;
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            previousPosition = rb.position;
        }
    }

    private void FixedUpdate()
    {
        if (smashManager != null &&
            smashManager.IsSmashed)
        {
            BreakDronesOnSmashPath();
        }

        previousPosition = rb.position;
    }

    private void BreakDronesOnSmashPath()
    {
        Vector2 currentPosition =
            rb.position;

        Vector2 movement =
            currentPosition -
            previousPosition;

        float distance =
            movement.magnitude;

        // 移動経路上のDroneを検出
        if (distance > 0.001f)
        {
            RaycastHit2D[] hits =
                Physics2D.CircleCastAll(
                    previousPosition,
                    smashBreakRadius,
                    movement.normalized,
                    distance,
                    droneLayer
                );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                BreakDrone(
                    hit.collider
                );
            }
        }

        // 現在位置に重なっているDroneも検出
        Collider2D[] overlaps =
            Physics2D.OverlapCircleAll(
                currentPosition,
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

    private void BreakDrone(
        Collider2D target
    )
    {
        Drone drone =
            target.GetComponent<Drone>();

        if (drone == null)
        {
            drone =
                target.GetComponentInParent<Drone>();
        }

        if (drone != null)
        {
            drone.BreakDrone();
        }
    }

    public void ResetPosition()
    {
        rb.linearVelocity =
            Vector2.zero;

        rb.position =
            Vector2.zero;

        previousPosition =
            rb.position;

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
            new Vector2(x, y)
                .normalized;

        rb.linearVelocity =
            direction * baseSpeed;

        currentSpeed = baseSpeed;
    }

    public void IncreaseSpeed(
        float amount
    )
    {
        float targetSpeed =
            currentSpeed + amount;

        if (ignoreMaxSpeed)
        {
            currentSpeed =
                targetSpeed;
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

    public void SetSpeed(
        float newSpeed
    )
    {
        if (ignoreMaxSpeed)
        {
            currentSpeed =
                newSpeed;
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
        if (rb.linearVelocity.sqrMagnitude
            <= 0.001f)
        {
            return;
        }

        rb.linearVelocity =
            rb.linearVelocity.normalized *
            currentSpeed;
    }

    public Vector2 velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    public void GivePowerUp(
        float multiplier
    )
    {
        hasPowerUp = true;
        powerMultiplier = multiplier;
    }

    public void ResetAndStartWithDelay(
        float delay
    )
    {
        StartCoroutine(
            StartAfterDelay(delay)
        );
    }

    private IEnumerator StartAfterDelay(
        float delay
    )
    {
        ResetPosition();

        yield return
            new WaitForSeconds(delay);

        AddStartingForce();
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