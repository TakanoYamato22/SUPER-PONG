using UnityEngine;
using System.Collections;

public abstract class BaseSmashController : MonoBehaviour
{
    public float chargeMoveDistance = 0.5f;
    public float chargeTime = 0.4f;

    protected float charge = 0f;
    protected bool isCharging = false;
    protected int chargeDir = 0;

    protected bool isSmashing = false;
    protected float cooldownTimer = 0f;

    protected Paddle paddle;
    protected Vector3 originalPos;

    protected Ball ball;
    protected BallSmashManager smashManager;

    public bool IsSmashing => isSmashing;

    protected virtual void Awake()
    {
        paddle = GetComponent<Paddle>();
        originalPos = transform.position;

        ball = FindAnyObjectByType<Ball>();
        smashManager = ball.GetComponent<BallSmashManager>();
    }

    protected void StartCharge(int dir)
    {
        if (!isCharging)
        {
            isCharging = true;
            charge = 0f;
            chargeDir = dir;
            originalPos = transform.position;
        }

        charge += Time.deltaTime;

        float offset = Mathf.Lerp(0f, chargeMoveDistance, charge / chargeTime);
        transform.position = originalPos + new Vector3(-chargeDir * offset, 0f, 0f);
    }

    protected void StopCharge()
    {
        if (!isCharging) return;

        isCharging = false;
        charge = 0f;

        StartCoroutine(ReturnToOriginalPos());
    }

    private IEnumerator ReturnToOriginalPos()
    {
        while (Vector3.Distance(transform.position, originalPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originalPos,
                8f * Time.deltaTime
            );

            yield return null;
        }

        transform.position = originalPos;
    }

    protected void Smash()
    {
        isSmashing = true;
        smashManager.ApplySmash();

        StopCharge();
        Invoke(nameof(EndSmash), 5.0f);
    }

    private void EndSmash()
    {
        isSmashing = false;
        smashManager.EndSmash();
    }
}
