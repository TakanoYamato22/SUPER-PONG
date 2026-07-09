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

    private bool isCharging = false;
    private bool isMoving = false;

    private float startX;
    private float cooldownTimer = 0f;

    public bool CanSmashNow()
    {
        return isCharging && smashZone != null && smashZone.CanSmash;
    }

    private void Awake()
    {
        startX = transform.position.x;
    }

    private void Update()
    {
        if (Input.GetKey(smashKey))
        {
            isCharging = true;

            Vector3 pos = transform.position;
            pos.x = startX + smashDirection * moveDistance;
            transform.position = Vector3.MoveTowards(
                transform.position,
                pos,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            isCharging = false;

            Vector3 pos = transform.position;
            pos.x = startX;
            transform.position = Vector3.MoveTowards(
                transform.position,
                pos,
                moveSpeed * Time.deltaTime
            );
        }
    }

    public void SuccessSmash()
    {
        isCharging = false;
<<<<<<< HEAD
<<<<<<< HEAD
        cooldownTimer = cooldownTime;
=======
>>>>>>> parent of a243831 (a)
=======
>>>>>>> parent of 094f2fa (Merge pull request #42 from TakanoYamato22/sota2)
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