
using System.Collections;
using UnityEngine;

public class AtlantisBossController : BossController
{
    [Header("ëÊ1å`ë‘")]
    [SerializeField] private float firstMoveInterval = 2f;

    [Header("ëÊ2å`ë‘")]
    [SerializeField] private float secondMoveInterval = 1f;

    [Header("ÉXÉ^Éì")]
    [SerializeField] private bool isStunned = false;

    // ========================================
    // é_ëfÉMÉ~ÉbÉN
    // ========================================

    [Header("é_ëfÉMÉ~ÉbÉN")]
    [SerializeField] private BallOxygenManager oxygenManager;
    [SerializeField] private OxygenSpawner oxygenSpawner;

    [SerializeField, Range(0f, 1f)]
    private float oxygenStartHpRatio = 1f;

    // ========================================
    // äCó¨ÉMÉ~ÉbÉN
    // ========================================

    [Header("äCó¨ÉMÉ~ÉbÉN")]
    [SerializeField] private OceanCurrentManager oceanCurrentManager;

    [SerializeField, Range(0f, 1f)]
    private float oceanStartHpRatio = 0.75f;

    // ========================================
    // ÉNÉäÅ[É`ÉÉÅ[ÉMÉ~ÉbÉN
    // ========================================

    [Header("ÉNÉäÅ[É`ÉÉÅ[ÉMÉ~ÉbÉN")]
    [SerializeField] private CreatureSpawner creatureSpawner;

    [SerializeField, Range(0f, 1f)]
    private float creatureStartHpRatio = 0.5f;

    // ========================================
    // âQí™ÉMÉ~ÉbÉN
    // ========================================

    [Header("âQí™ÉMÉ~ÉbÉN")]
    [SerializeField] private WhirlpoolSpawner whirlpoolSpawner;

    [SerializeField, Range(0f, 1f)]
    private float whirlpoolStartHpRatio = 0.4f;

    // ========================================
    // êÖà≥è’åÇîg
    // ========================================

    [Header("êÖà≥è’åÇîg")]
    [SerializeField] private WaterShockwave shockwavePrefab;
    [SerializeField] private Transform shockwaveOrigin;
    [SerializeField] private Transform bossVisual;

    [SerializeField, Range(0f, 1f)]
    private float shockwaveStartHpRatio = 0.25f;

    [SerializeField, Min(0.1f)]
    private float shockwaveInterval = 12f;

    [SerializeField, Min(0f)]
    private float chargeDuration = 1.5f;

    [SerializeField, Min(0f)]
    private float shakePower = 0.08f;

    [SerializeField, Min(1f)]
    private float jumpScale = 1.3f;

    [SerializeField, Min(0.01f)]
    private float jumpDuration = 0.3f;

    // ========================================
    // ì‡ïîïœêî
    // ========================================

    private float stunTimer;
    private float directionTimer;
    private float shockwaveTimer;

    private Vector2 moveDirection = Vector2.up;
    private Vector3 initialPosition;

    private bool isAttacking;
    private Coroutine shockwaveRoutine;

    private bool oxygenStarted;
    private bool oceanStarted;
    private bool creatureStarted;
    private bool whirlpoolStarted;

    protected override void Start()
    {
        base.Start();

        initialPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if (!BattleStarted ||
            IsDead ||
            IsPhaseChanging ||
            !IsSecondPhase)
        {
            return;
        }

        if (MaxHP <= 0f)
            return;

        float hpRatio = CurrentHP / MaxHP;

        // HPÇ…âûÇ∂ÇƒäeÉMÉ~ÉbÉNÇäJén
        CheckHpGimmicks(hpRatio);

        // êÖà≥è’åÇîg
        UpdateShockwave(hpRatio);
    }

    // ========================================
    // HPÇ…ÇÊÇÈÉMÉ~ÉbÉNâã÷
    // ========================================

    private void CheckHpGimmicks(float hpRatio)
    {
        // é_ëf
        if (!oxygenStarted &&
            hpRatio <= oxygenStartHpRatio)
        {
            oxygenStarted = true;

            if (oxygenManager != null)
                oxygenManager.StartOxygenGimmick();

            if (oxygenSpawner != null)
                oxygenSpawner.StartSpawning();

            Debug.Log("é_ëfÉMÉ~ÉbÉNâã÷ÅI");
        }

        // äCó¨
        if (!oceanStarted &&
            hpRatio <= oceanStartHpRatio)
        {
            oceanStarted = true;

            if (oceanCurrentManager != null)
                oceanCurrentManager.StartOceanCurrent();

            Debug.Log("äCó¨ÉMÉ~ÉbÉNâã÷ÅI");
        }

        // ÉNÉäÅ[É`ÉÉÅ[
        if (!creatureStarted &&
            hpRatio <= creatureStartHpRatio)
        {
            creatureStarted = true;

            if (creatureSpawner != null)
                creatureSpawner.StartSpawning();

            Debug.Log("ÉNÉäÅ[É`ÉÉÅ[ÉMÉ~ÉbÉNâã÷ÅI");
        }

        // âQí™
        if (!whirlpoolStarted &&
            hpRatio <= whirlpoolStartHpRatio)
        {
            whirlpoolStarted = true;

            if (whirlpoolSpawner != null)
                whirlpoolSpawner.StartSpawning();

            Debug.Log("âQí™ÉMÉ~ÉbÉNâã÷ÅI");
        }
    }

    // ========================================
    // êÖà≥è’åÇîgÇÃHPîªíË
    // ========================================

    private void UpdateShockwave(float hpRatio)
    {
        if (isAttacking || isStunned)
            return;

        if (hpRatio > shockwaveStartHpRatio)
            return;

        shockwaveTimer += Time.deltaTime;

        if (shockwaveTimer >= shockwaveInterval)
        {
            shockwaveTimer = 0f;

            shockwaveRoutine =
                StartCoroutine(ShockwaveAttackRoutine());
        }
    }

    // ========================================
    // ÉXÉ^Éì
    // ========================================

    private void FixedUpdate()
    {
        if (!isStunned)
            return;

        stunTimer -= Time.fixedDeltaTime;

        if (stunTimer <= 0f)
        {
            isStunned = false;
            stunTimer = 0f;
        }
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = Mathf.Max(0f, duration);
    }

    // ========================================
    // É{ÉXà⁄ìÆ
    // ========================================

    protected override void Move()
    {
        if (isStunned || isAttacking)
            return;

        float interval = IsSecondPhase
            ? secondMoveInterval
            : firstMoveInterval;

        directionTimer += Time.deltaTime;

        if (directionTimer >= interval)
        {
            directionTimer = 0f;
            ChangeDirection();
        }

        Vector3 nextPosition = transform.position +
            (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        float minX = initialPosition.x - moveRangeX;
        float maxX = initialPosition.x + moveRangeX;
        float minY = initialPosition.y - moveRangeY;
        float maxY = initialPosition.y + moveRangeY;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);

        if ((nextPosition.x <= minX && moveDirection.x < 0f) ||
            (nextPosition.x >= maxX && moveDirection.x > 0f))
        {
            moveDirection.x *= -1f;
        }

        if ((nextPosition.y <= minY && moveDirection.y < 0f) ||
            (nextPosition.y >= maxY && moveDirection.y > 0f))
        {
            moveDirection.y *= -1f;
        }

        transform.position = nextPosition;
    }

    private void ChangeDirection()
    {
        if (!IsSecondPhase)
        {
            moveDirection = Random.value < 0.5f
                ? Vector2.up
                : Vector2.down;
        }
        else
        {
            moveDirection = Random.insideUnitCircle.normalized;

            if (moveDirection.sqrMagnitude < 0.01f)
                moveDirection = Vector2.up;
        }
    }

    // ========================================
    // êÖà≥è’åÇîgÇÃçUåÇââèo
    // ========================================

    private IEnumerator ShockwaveAttackRoutine()
    {
        isAttacking = true;

        Transform visual = bossVisual != null
            ? bossVisual
            : transform;

        Vector3 originalLocalPosition = visual.localPosition;
        Vector3 originalScale = visual.localScale;

        // á@ í‚é~ÇµÇƒêkÇ¶ÇÈ
        float timer = 0f;

        while (timer < chargeDuration)
        {
            if (!BattleStarted || IsDead || IsPhaseChanging)
                break;

            timer += Time.deltaTime;

            visual.localPosition = originalLocalPosition +
                (Vector3)(Random.insideUnitCircle * shakePower);

            yield return null;
        }

        visual.localPosition = originalLocalPosition;

        if (!BattleStarted || IsDead || IsPhaseChanging)
        {
            visual.localScale = originalScale;
            isAttacking = false;
            shockwaveRoutine = null;
            yield break;
        }

        // áA Ç“ÇÂÇÒÇ∆ägëÂ
        timer = 0f;

        while (timer < jumpDuration)
        {
            if (!BattleStarted || IsDead || IsPhaseChanging)
                break;

            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                timer / jumpDuration
            );

            float scaleFactor = progress <= 0.5f
                ? Mathf.Lerp(
                    1f,
                    jumpScale,
                    progress * 2f
                )
                : Mathf.Lerp(
                    jumpScale,
                    1f,
                    (progress - 0.5f) * 2f
                );

            visual.localScale =
                originalScale * scaleFactor;

            yield return null;
        }

        visual.localScale = originalScale;

        if (!BattleStarted || IsDead || IsPhaseChanging)
        {
            isAttacking = false;
            shockwaveRoutine = null;
            yield break;
        }

        // áB è’åÇîgî≠éÀ
        if (shockwavePrefab != null)
        {
            Vector3 spawnPosition =
                shockwaveOrigin != null
                    ? shockwaveOrigin.position
                    : transform.position;

            Instantiate(
                shockwavePrefab,
                spawnPosition,
                Quaternion.identity
            );

            Debug.Log("êÖà≥è’åÇîgî≠éÀÅI");
        }
        else
        {
            Debug.LogWarning(
                "WaterShockwave PrefabÇ™ñ¢ê›íËÇ≈Ç∑ÅB",
                this
            );
        }

        isAttacking = false;
        shockwaveRoutine = null;
    }

    private void OnDisable()
    {
        if (shockwaveRoutine != null)
        {
            StopCoroutine(shockwaveRoutine);
            shockwaveRoutine = null;
        }

        isAttacking = false;
    }

    // ========================================
    // É_ÉÅÅ[ÉWèàóù
    // ========================================

    public override void TakeDamage(float damage)
    {
        Debug.Log(
            $"ÅyAtlantisÅzÉ_ÉÅÅ[ÉWèàóùÇ™åƒÇŒÇÍÇΩÅI " +
            $"damage={damage}, " +
            $"BattleStarted={BattleStarted}, " +
            $"HP={CurrentHP}"
        );

        base.TakeDamage(damage);

        Debug.Log(
            $"ÅyAtlantisÅzèàóùå„HP={CurrentHP}"
        );
    }
}
