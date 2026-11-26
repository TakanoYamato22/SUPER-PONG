using UnityEngine;
using System.Collections;

public class SmashController : MonoBehaviour
{
    // ========================================
    // 入力
    // ========================================

    [Header("Input")]
    [SerializeField]
    private KeyCode smashKey = KeyCode.D;

    // ========================================
    // スマッシュ判定
    // ========================================

    [Header("Smash Zone")]
    [SerializeField]
    private SmashZone smashZone;

    // ========================================
    // 移動
    // ========================================

    [Header("Move")]
    [SerializeField]
    private float moveDistance = 0.5f;

    [SerializeField]
    private float moveSpeed = 12f;

    [Header("Direction")]
    [SerializeField]
    private int smashDirection = 1;

    // ========================================
    // クールタイム
    // ========================================

    [Header("Cooldown")]
    [SerializeField]
    private float cooldownTime = 7f;

    private bool isCharging;
    private bool isMoving;

    private float startX;
    private float cooldownTimer;

    // ========================================
    // サーカスステージ：スマッシュ封印
    // ========================================

    private bool smashLocked;
    private Coroutine smashLockCoroutine;

    public bool IsCharging => isCharging;
    public bool IsSmashLocked => smashLocked;

    public float CooldownTime => cooldownTime;
    public float CooldownTimer => cooldownTimer;

    // ========================================
    // スマッシュ可能判定
    // ========================================

    public bool CanSmashNow()
    {
        return
            !smashLocked &&
            isCharging &&
            cooldownTimer <= 0f &&
            smashZone != null &&
            smashZone.CanSmash;
    }

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        startX = transform.position.x;
    }

    // ========================================
    // 更新
    // ========================================

    private void Update()
    {
        UpdateCooldown();
        HandleSmashInput();
    }

    // ========================================
    // クールタイム更新
    // ========================================

    private void UpdateCooldown()
    {
        if (cooldownTimer <= 0f)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0f)
        {
            cooldownTimer = 0f;
        }
    }

    // ========================================
    // スマッシュ入力
    // ========================================

    private void HandleSmashInput()
    {
        // スマッシュ封印中
        if (smashLocked)
        {
            isCharging = false;
            ReturnTowardsStart();
            return;
        }

        // クールタイム中
        if (cooldownTimer > 0f)
        {
            isCharging = false;
            ReturnTowardsStart();
            return;
        }

        if (Input.GetKey(smashKey))
        {
            isCharging = true;

            Vector3 targetPosition = transform.position;

            targetPosition.x =
                startX +
                smashDirection *
                moveDistance;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            isCharging = false;
            ReturnTowardsStart();
        }
    }

    // ========================================
    // 通常位置へ戻る
    // ========================================

    private void ReturnTowardsStart()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = startX;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    // ========================================
    // キャラクター設定反映
    // ========================================

    public void ApplyCharacterSettings(
        float newCooldownTime,
        float newMoveDistance,
        float newMoveSpeed
    )
    {
        cooldownTime = Mathf.Max(
            0f,
            newCooldownTime
        );

        // マイナス方向への移動も許可
        moveDistance = newMoveDistance;

        moveSpeed = Mathf.Max(
            0.1f,
            newMoveSpeed
        );

        Debug.Log(
            gameObject.name +
            " Smash設定反映" +
            " / CT: " + cooldownTime +
            " / Distance: " + moveDistance +
            " / Speed: " + moveSpeed
        );
    }

    // ========================================
    // スマッシュ成功
    // ========================================

    public void SuccessSmash()
    {
        // 封印中はスマッシュ成功を受け付けない
        if (smashLocked)
        {
            return;
        }

        isCharging = false;
        cooldownTimer = cooldownTime;

        StartCoroutine(ReturnX());
    }

    private IEnumerator ReturnX()
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        while (
            Mathf.Abs(
                transform.position.x - startX
            ) > 0.01f
        )
        {
            Vector3 position = transform.position;

            position.x = Mathf.MoveTowards(
                position.x,
                startX,
                moveSpeed * Time.deltaTime
            );

            transform.position = position;

            yield return null;
        }

        Vector3 finalPosition = transform.position;
        finalPosition.x = startX;

        transform.position = finalPosition;

        isMoving = false;
    }

    // ========================================
    // スマッシュ封印
    // ========================================

    public void LockSmash(float duration)
    {
        // 前回の封印タイマーを停止
        if (smashLockCoroutine != null)
        {
            StopCoroutine(smashLockCoroutine);
            smashLockCoroutine = null;
        }

        if (duration <= 0f)
        {
            UnlockSmash();
            return;
        }

        smashLocked = true;
        isCharging = false;

        Debug.Log(
            $"{gameObject.name}：スマッシュ封印！" +
            $"{duration}秒間",
            this
        );

        smashLockCoroutine = StartCoroutine(
            SmashLockRoutine(duration)
        );
    }

    private IEnumerator SmashLockRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        UnlockSmash();
    }

    // ========================================
    // スマッシュ封印解除
    // ========================================

    public void UnlockSmash()
    {
        if (smashLockCoroutine != null)
        {
            StopCoroutine(smashLockCoroutine);
            smashLockCoroutine = null;
        }

        smashLocked = false;
        isCharging = false;

        Debug.Log(
            $"{gameObject.name}：スマッシュ封印解除！",
            this
        );
    }

    // ========================================
    // 無効化時
    // ========================================

    private void OnDisable()
    {
        smashLocked = false;
        isCharging = false;
        smashLockCoroutine = null;
    }
}