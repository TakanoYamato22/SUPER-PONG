using UnityEngine;
using System.Collections;

public class SmashController : MonoBehaviour
{
    // ==================================================
    // 入力
    // ==================================================

    [Header("Input")]
    [SerializeField] private KeyCode smashKey = KeyCode.D;

    // ==================================================
    // スマッシュ判定
    // ==================================================

    [Header("Smash Zone")]
    [SerializeField] private SmashZone smashZone;

    // ==================================================
    // 移動
    // ==================================================

    [Header("Move")]
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float moveSpeed = 12f;

    [Header("Direction")]
    [SerializeField] private int smashDirection = 1;

    // ==================================================
    // クールタイム
    // ==================================================

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 2f;

    private bool isCharging;
    private bool isMoving;

    private float startX;
    private float cooldownTimer;

    public bool IsCharging => isCharging;
    public float CooldownTime => cooldownTime;
    public float CooldownTimer => cooldownTimer;

    public bool CanSmashNow()
    {
        return
            isCharging &&
            cooldownTimer <= 0f &&
            smashZone != null &&
            smashZone.CanSmash;
    }

    private void Awake()
    {
        startX = transform.position.x;
    }

    private void Update()
    {
        UpdateCooldown();
        HandleSmashInput();
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer <= 0f)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0f)
        {
            cooldownTimer = 0f;
        }
    }

    private void HandleSmashInput()
    {
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

    // CharacterDataの数値を反映
    public void ApplyCharacterSettings(
        float newCooldownTime,
        float newMoveDistance,
        float newMoveSpeed
    )
    {
        cooldownTime = Mathf.Max(0f, newCooldownTime);
        moveDistance = Mathf.Max(0f, newMoveDistance);
        moveSpeed = Mathf.Max(0.1f, newMoveSpeed);

        Debug.Log(
            gameObject.name +
            " Smash設定反映" +
            " / CT: " + cooldownTime +
            " / Distance: " + moveDistance +
            " / Speed: " + moveSpeed
        );
    }

    public void SuccessSmash()
    {
        isCharging = false;
        cooldownTimer = cooldownTime;

        StartCoroutine(ReturnX());
    }

    private IEnumerator ReturnX()
    {
        if (isMoving)
            yield break;

        isMoving = true;

        while (
            Mathf.Abs(
                transform.position.x -
                startX
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
}