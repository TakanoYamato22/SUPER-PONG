using System.Collections;
using UnityEngine;

public class VolcanoBossController :
    BossController
{
    [Header("第2形態の移動")]
    [SerializeField]
    private float directionChangeInterval = 1.5f;

    [Header("第2形態の攻撃")]
    [SerializeField]
    private GameObject stonePrefab;

    [SerializeField]
    private Transform stoneSpawnPoint;

    [SerializeField]
    private float stoneFireInterval = 2f;

    [SerializeField]
    private float stoneSpeed = 5f;

    [Header("形態変化時の揺れ")]
    [SerializeField]
    private float shakePower = 0.15f;

    private bool secondPhaseBehaviourStarted;

    private Vector2 moveDirection;
    private float directionTimer;
    private float stoneTimer;

    private Vector3 basePosition;

    protected override void Start()
    {
        base.Start();

        basePosition =
            transform.position;

        /*
         * 第1形態は動かない。
         * 第2形態開始まで行動停止。
         */
        secondPhaseBehaviourStarted = false;

        ChangeMoveDirection();
    }

    protected override void Update()
    {
        /*
         * BossController側のUpdateは呼ばない。
         * 第1形態では絶対にMoveさせないため、
         * 第2形態開始後だけ独自行動を実行。
         */
        if (!secondPhaseBehaviourStarted)
        {
            return;
        }

        if (!BattleStarted ||
            IsDead ||
            IsPhaseChanging)
        {
            return;
        }

        Move();
        UpdateStoneAttack();
    }

    protected override void Move()
    {
        directionTimer -=
            Time.deltaTime;

        if (directionTimer <= 0f)
        {
            ChangeMoveDirection();
        }

        Vector3 movement =
            new Vector3(
                moveDirection.x,
                moveDirection.y,
                0f
            ) *
            moveSpeed *
            Time.deltaTime;

        transform.position +=
            movement;

        ClampPosition();
    }

    /// <summary>
    /// 形態変化演出が完全に終了した後に呼ぶ。
    /// </summary>
    public void StartMoveAfterPhaseChange()
    {
        secondPhaseBehaviourStarted =
            true;

        directionTimer = 0f;
        stoneTimer = stoneFireInterval;

        ChangeMoveDirection();

        Debug.Log(
            "火山Boss第2形態：移動と攻撃を開始",
            this
        );
    }

    private void ChangeMoveDirection()
    {
        moveDirection =
            Random.insideUnitCircle.normalized;

        directionTimer =
            directionChangeInterval;
    }

    private void ClampPosition()
    {
        Vector3 position =
            transform.position;

        position.x =
            Mathf.Clamp(
                position.x,
                basePosition.x - moveRangeX,
                basePosition.x + moveRangeX
            );

        position.y =
            Mathf.Clamp(
                position.y,
                basePosition.y - moveRangeY,
                basePosition.y + moveRangeY
            );

        transform.position =
            position;
    }

    private void UpdateStoneAttack()
    {
        if (stonePrefab == null ||
            stoneSpawnPoint == null)
        {
            return;
        }

        stoneTimer -=
            Time.deltaTime;

        if (stoneTimer > 0f)
        {
            return;
        }

        FireStone();

        stoneTimer =
            stoneFireInterval;
    }

    private void FireStone()
    {
        GameObject stone =
            Instantiate(
                stonePrefab,
                stoneSpawnPoint.position,
                Quaternion.identity
            );

        Rigidbody2D rb =
            stone.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            return;
        }

        Vector2 direction =
            Vector2.down;

        rb.linearVelocity =
            direction * stoneSpeed;
    }

    /// <summary>
    /// 形態変化開始時の揺れ。
    /// Time.timeScaleが0でも動く。
    /// </summary>
    public IEnumerator PlayPhaseChangeShake(
        float duration
    )
    {
        Vector3 startPosition =
            transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX =
                Random.Range(
                    -shakePower,
                    shakePower
                );

            float offsetY =
                Random.Range(
                    -shakePower,
                    shakePower
                );

            transform.position =
                startPosition +
                new Vector3(
                    offsetX,
                    offsetY,
                    0f
                );

            elapsed +=
                Time.unscaledDeltaTime;

            yield return null;
        }

        transform.position =
            startPosition;

        basePosition =
            startPosition;
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        bool previousState =
            secondPhaseBehaviourStarted;

        secondPhaseBehaviourStarted = false;

        yield return new WaitForSeconds(duration);

        if (IsSecondPhase)
        {
            secondPhaseBehaviourStarted =
                previousState;
        }
    }
}