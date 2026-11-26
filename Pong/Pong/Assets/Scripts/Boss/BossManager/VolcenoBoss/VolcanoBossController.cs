
using System.Collections;
using UnityEngine;

public class VolcanoBossController : BossController
{
    [Header("第2形態の移動")]
    [SerializeField] private float directionChangeInterval = 1.5f;

    [Header("移動開始HP")]
    [SerializeField, Range(0f, 1f)]
    private float moveStartHpRatio = 0.3f;

    [Header("氷柱ギミック")]
    [SerializeField] private IcicleSpawner icicleSpawner;
    [SerializeField, Range(0f, 1f)] private float icicleStartHpRatio = 0.5f;

    // ★追加：噴火ギミック
    [Header("噴火ギミック")]
    [SerializeField] private EruptionSpawner eruptionSpawner;
    [SerializeField, Range(0f, 1f)] private float eruptionStartHpRatio = 0.75f;

    [Header("形態変化時の揺れ")]
    [SerializeField] private float shakePower = 0.15f;

    private bool secondPhaseBehaviourStarted;
    private bool icicleStarted;

    // ★追加
    private bool eruptionStarted;

    private Vector2 moveDirection;
    private float directionTimer;
    private Vector3 basePosition;

    protected override void Start()
    {
        base.Start();

        basePosition = transform.position;
        secondPhaseBehaviourStarted = false;

        ChangeMoveDirection();
    }



    protected override void Update()
    {
        if (!BattleStarted ||
            IsDead ||
            IsPhaseChanging)
            return;

        if (IsSecondPhase &&
            MaxHP > 0f &&
            CurrentHP <= MaxHP * moveStartHpRatio)
        {
            if (!secondPhaseBehaviourStarted)
            {
                StartMoveAfterPhaseChange();
            }

            Move();
        }

        UpdateEruptionAttack();
        UpdateIcicleAttack();
    }


    protected override void Move()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer <= 0f)
        {
            ChangeMoveDirection();
        }

        Vector3 pos = transform.position;

        pos += (Vector3)moveDirection
            * moveSpeed
            * Time.deltaTime;

        float minX = basePosition.x - moveRangeX;
        float maxX = basePosition.x + moveRangeX;

        float minY = basePosition.y - moveRangeY;
        float maxY = basePosition.y + moveRangeY;

        // 左右の端に到達したら反射
        if (pos.x <= minX || pos.x >= maxX)
        {
            moveDirection.x *= -1f;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }

        // 上下の端に到達したら反射
        if (pos.y <= minY || pos.y >= maxY)
        {
            moveDirection.y *= -1f;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        transform.position = pos;
    }

    public void StartMoveAfterPhaseChange()
    {
        secondPhaseBehaviourStarted = true;
        directionTimer = 0f;
        ChangeMoveDirection();
    }

    private void ChangeMoveDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        directionTimer = directionChangeInterval;
    }

    // ==========================================
    // ★追加：HP75%以下で噴火開始
    // ==========================================

    private void UpdateEruptionAttack()
    {
        if (eruptionStarted || eruptionSpawner == null)
            return;

        if (!IsSecondPhase || MaxHP <= 0f)
            return;

        if (CurrentHP > MaxHP * eruptionStartHpRatio)
            return;

        eruptionStarted = true;
        eruptionSpawner.StartSpawning();

        Debug.Log("BossのHPが75％以下！噴火攻撃開始！");
    }

    // ==========================================
    // HP50%以下で氷柱開始（既存処理）
    // ==========================================

    private void UpdateIcicleAttack()
    {
        if (icicleStarted || icicleSpawner == null)
            return;

        if (!IsSecondPhase || MaxHP <= 0f)
            return;

        if (CurrentHP > MaxHP * icicleStartHpRatio)
            return;

        icicleStarted = true;
        icicleSpawner.StartSpawning();

        Debug.Log("BossのHPが50％以下！氷柱攻撃開始！");
    }

    public IEnumerator PlayPhaseChangeShake(float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = startPosition +
                new Vector3(
                    Random.Range(-shakePower, shakePower),
                    Random.Range(-shakePower, shakePower),
                    0f
                );

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.position = startPosition;
        basePosition = startPosition;
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        bool previousState = secondPhaseBehaviourStarted;
        secondPhaseBehaviourStarted = false;

        yield return new WaitForSeconds(duration);

        if (IsSecondPhase)
            secondPhaseBehaviourStarted = previousState;
    }
}
