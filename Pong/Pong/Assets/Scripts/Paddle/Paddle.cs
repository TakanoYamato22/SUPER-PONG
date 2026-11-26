using UnityEngine;
using System.Collections;

public abstract class Paddle : MonoBehaviour
{
    [Header("通常移動")]
    public float speed = 5f;

    [Header("ボスギミック用スピード")]
    [SerializeField] private float bossSpeedMultiplier = 1.5f;

    private float normalSpeed;
    private bool bossSpeedUp;

    [Header("慣性")]
    [Tooltip("最初から慣性を有効にする")]
    [SerializeField] private bool inertiaEnabled;

    [Tooltip("入力中に速度が上がる強さ")]
    [Min(0f)]
    [SerializeField] private float acceleration = 18f;

    [Tooltip("入力を離した後に止まるまでの強さ")]
    [Min(0f)]
    [SerializeField] private float deceleration = 7f;

    [Tooltip("反対方向を入力したときの切り返しの強さ")]
    [Min(0f)]
    [SerializeField] private float turnAcceleration = 28f;

    // ========================================
    // 水中ステージ
    // ========================================

    [Header("水中ステージ")]
    [Tooltip("水中ステージのギミックを有効にする")]
    [SerializeField] private bool underwaterEnabled = false;

    [Tooltip("上昇速度の倍率")]
    [SerializeField, Range(0f, 1f)]
    private float underwaterUpMultiplier = 0.6f;

    [Tooltip("入力していない時の沈下速度")]
    [Min(0f)]
    [SerializeField] private float underwaterSinkSpeed = 2f;

    // ========================================
    // Smash
    // ========================================

    [Header("Smash")]
    public float smashPower = 2f;

    private Vector3 originalScale;
    private Coroutine growCoroutine;
    private Coroutine speedCoroutine;

    private float inertiaVelocity;

    // ========================================
    // サーカスステージ：操作反転
    // ========================================

    private bool reverseEnabled;
    private Coroutine reverseCoroutine;

    public bool IsReversed => reverseEnabled;

    protected virtual void Awake()
    {
        originalScale = transform.localScale;

        normalSpeed = speed;
    }

    // ========================================
    // 移動速度計算
    // ========================================

    /// <summary>
    /// Paddleの移動速度を計算する。
    /// moveInputには-1、0、1を渡す。
    /// </summary>
    protected float CalculateMoveVelocity(
        float moveInput,
        float deltaTime
    )
    {
        moveInput = Mathf.Clamp(
            moveInput,
            -1f,
            1f
        );

        // サーカスステージ：操作反転
        if (reverseEnabled)
        {
            moveInput *= -1f;
        }

        // ====================================
        // 水中ステージ専用処理
        // ====================================

        if (underwaterEnabled)
        {
            float velocity;

            if (moveInput > 0f)
            {
                velocity =
                    moveInput *
                    speed *
                    underwaterUpMultiplier;
            }
            else if (moveInput < 0f)
            {
                velocity = moveInput * speed;
            }
            else
            {
                velocity = -underwaterSinkSpeed;
            }

            inertiaVelocity = velocity;

            return velocity;
        }

        // ====================================
        // 通常ステージの処理
        // ====================================

        if (!inertiaEnabled)
        {
            inertiaVelocity = moveInput * speed;
            return inertiaVelocity;
        }

        float targetVelocity = moveInput * speed;

        if (Mathf.Abs(moveInput) < 0.01f)
        {
            inertiaVelocity = Mathf.MoveTowards(
                inertiaVelocity,
                0f,
                deceleration * deltaTime
            );

            return inertiaVelocity;
        }

        bool changingDirection =
            Mathf.Abs(inertiaVelocity) > 0.01f &&
            Mathf.Sign(inertiaVelocity) !=
            Mathf.Sign(targetVelocity);

        float currentAcceleration =
            changingDirection
                ? turnAcceleration
                : acceleration;

        inertiaVelocity = Mathf.MoveTowards(
            inertiaVelocity,
            targetVelocity,
            currentAcceleration * deltaTime
        );

        return inertiaVelocity;
    }

    // ========================================
    // サーカスステージ：操作反転
    // ========================================

    public void Reverse(float duration)
    {
        if (reverseCoroutine != null)
        {
            StopCoroutine(reverseCoroutine);
            reverseCoroutine = null;
        }

        if (duration <= 0f)
        {
            SetReverseEnabled(false);
            return;
        }

        SetReverseEnabled(true);

        reverseCoroutine = StartCoroutine(
            ReverseRoutine(duration)
        );
    }

    private IEnumerator ReverseRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        SetReverseEnabled(false);

        reverseCoroutine = null;
    }

    public void SetReverseEnabled(bool enabled)
    {
        reverseEnabled = enabled;

        // 反転切り替え時に慣性をリセット
        StopInertia();

        Debug.Log(
            $"{gameObject.name}：操作反転" +
            (enabled ? "ON" : "OFF"),
            this
        );
    }

    public void StopReverse()
    {
        if (reverseCoroutine != null)
        {
            StopCoroutine(reverseCoroutine);
            reverseCoroutine = null;
        }

        SetReverseEnabled(false);
    }

    private void OnDisable()
    {
        reverseEnabled = false;
        reverseCoroutine = null;
        inertiaVelocity = 0f;
    }

    // ========================================
    // 水中ギミック切り替え
    // ========================================

    public void SetUnderwaterEnabled(bool enabled)
    {
        underwaterEnabled = enabled;

        StopInertia();

        Debug.Log(
            $"{gameObject.name}：水中ギミック" +
            (enabled ? "ON" : "OFF"),
            this
        );
    }

    public bool IsUnderwaterEnabled()
    {
        return underwaterEnabled;
    }

    // ========================================
    // 慣性
    // ========================================

    public void EnableInertia()
    {
        inertiaEnabled = true;

        Debug.Log(
            $"{gameObject.name}：慣性ON",
            this
        );
    }

    public void DisableInertia()
    {
        inertiaEnabled = false;
        inertiaVelocity = 0f;

        Debug.Log(
            $"{gameObject.name}：慣性OFF",
            this
        );
    }

    public void SetInertiaEnabled(bool enabled)
    {
        if (enabled)
        {
            EnableInertia();
        }
        else
        {
            DisableInertia();
        }
    }

    public void StopInertia()
    {
        inertiaVelocity = 0f;
    }

    public bool IsInertiaEnabled()
    {
        return inertiaEnabled;
    }

    // ========================================
    // ボスギミック用高速化
    // ========================================

    public void EnableBossSpeedUp()
    {
        bossSpeedUp = true;

        DisableInertia();

        speed = normalSpeed * bossSpeedMultiplier;

        Debug.Log(
            $"{gameObject.name}：高速モードON",
            this
        );
    }

    public void DisableBossSpeedUp()
    {
        bossSpeedUp = false;

        speed = normalSpeed;

        EnableInertia();
    }

    public bool IsBossSpeedUp()
    {
        return bossSpeedUp;
    }

    // ========================================
    // 位置リセット
    // ========================================

    public void ResetPosition()
    {
        inertiaVelocity = 0f;

        transform.position = new Vector2(
            transform.position.x,
            0f
        );
    }

    // ========================================
    // パドル縮小
    // ========================================

    public void Shrink(float duration)
    {
        PaddleSizeController size =
            GetComponent<PaddleSizeController>();

        if (size != null)
        {
            size.Shrink(duration);
        }
    }

    // ========================================
    // パドル拡大
    // ========================================

    public void Grow(float duration)
    {
        if (growCoroutine != null)
        {
            StopCoroutine(growCoroutine);
        }

        growCoroutine = StartCoroutine(
            GrowRoutine(duration)
        );
    }

    private IEnumerator GrowRoutine(float duration)
    {
        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * 1.5f,
            originalScale.z
        );

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;

        growCoroutine = null;
    }

    // ========================================
    // 一時的な速度上昇
    // ========================================

    public void SpeedUp(float duration)
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }

        speedCoroutine = StartCoroutine(
            SpeedUpRoutine(duration)
        );
    }

    private IEnumerator SpeedUpRoutine(float duration)
    {
        float originalSpeed = speed;

        speed = originalSpeed * 1.3f;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;

        speedCoroutine = null;
    }
}