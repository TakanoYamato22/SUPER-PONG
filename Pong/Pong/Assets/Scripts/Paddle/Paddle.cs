using UnityEngine;
using System.Collections;

public abstract class Paddle : MonoBehaviour
{
    [Header("通常移動")]
    public float speed = 5f;

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

    [Header("Smash")]
    public float smashPower = 2f;

    private Vector3 originalScale;
    private Coroutine growCoroutine;
    private Coroutine speedCoroutine;

    private float inertiaVelocity;

    protected virtual void Awake()
    {
        originalScale = transform.localScale;
    }

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

        // 慣性OFFなら、今までどおり即座に移動
        if (!inertiaEnabled)
        {
            inertiaVelocity =
                moveInput * speed;

            return inertiaVelocity;
        }

        float targetVelocity =
            moveInput * speed;

        // 入力なしなら徐々に停止
        if (Mathf.Abs(moveInput) < 0.01f)
        {
            inertiaVelocity =
                Mathf.MoveTowards(
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

        inertiaVelocity =
            Mathf.MoveTowards(
                inertiaVelocity,
                targetVelocity,
                currentAcceleration * deltaTime
            );

        return inertiaVelocity;
    }

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

    public void ResetPosition()
    {
        inertiaVelocity = 0f;

        transform.position =
            new Vector2(
                transform.position.x,
                0f
            );
    }

    public void Shrink(float duration)
    {
        PaddleSizeController size =
            GetComponent<PaddleSizeController>();

        if (size != null)
        {
            size.Shrink(duration);
        }
    }

    public void Grow(float duration)
    {
        if (growCoroutine != null)
        {
            StopCoroutine(growCoroutine);
        }

        growCoroutine =
            StartCoroutine(
                GrowRoutine(duration)
            );
    }

    private IEnumerator GrowRoutine(float duration)
    {
        transform.localScale =
            new Vector3(
                originalScale.x,
                originalScale.y * 1.5f,
                originalScale.z
            );

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
        growCoroutine = null;
    }

    public void SpeedUp(float duration)
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }

        speedCoroutine =
            StartCoroutine(
                SpeedUpRoutine(duration)
            );
    }

    private IEnumerator SpeedUpRoutine(float duration)
    {
        float originalSpeed = speed;

        speed =
            originalSpeed * 1.3f;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        speedCoroutine = null;
    }
}