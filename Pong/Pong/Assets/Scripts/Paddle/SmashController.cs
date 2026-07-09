using UnityEngine;
using System.Collections;

public class SmashController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode smashKey = KeyCode.D;

    [Header("Smash Zone")]
    [SerializeField] private SmashZone smashZone;

    [Header("Move")]
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float moveSpeed = 12f;

    [Header("Direction")]
    [SerializeField] private int smashDirection = 1;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 2.0f;

    private bool isCharging = false;
    private bool isMoving = false;

    private float cooldownTimer = 0f;
    private float startX;

    public bool CanSmashNow()
    {
        return isCharging && cooldownTimer <= 0f && smashZone != null && smashZone.CanSmash;
    }

    private void Awake()
    {
        startX = transform.position.x;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(smashKey) && cooldownTimer <= 0f)
        {
            isCharging = true;

            Vector3 targetPos = transform.position;
            targetPos.x = startX + smashDirection * moveDistance;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            isCharging = false;

            Vector3 targetPos = transform.position;
            targetPos.x = startX;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
    }

    public void SuccessSmash()
    {
        isCharging = false;
        cooldownTimer = cooldownTime;

        StartCoroutine(ReturnX());
    }

    private IEnumerator ReturnX()
    {
        if (isMoving) yield break;

        isMoving = true;

        while (Mathf.Abs(transform.position.x - startX) > 0.01f)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, startX, moveSpeed * Time.deltaTime);
            transform.position = pos;

            yield return null;
        }

        Vector3 finalPos = transform.position;
        finalPos.x = startX;
        transform.position = finalPos;

        isMoving = false;
    }
}