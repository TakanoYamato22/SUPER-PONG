using UnityEngine;
using System.Collections;

public class GiusController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveRangeX = 3f;
    [SerializeField] private float moveRangeY = 2f;
    [SerializeField] private float startWaitTime = 3f;

    private bool canMove;
    private Vector3 basePosition;
    private Vector3 targetPosition;

    public void StartMove()
    {
        basePosition = transform.position;
        StartCoroutine(StartMoveAfterWait());
    }

    private IEnumerator StartMoveAfterWait()
    {
        canMove = false;

        yield return new WaitForSeconds(startWaitTime);

        SetNewTarget();
        canMove = true;
    }

    public void StopMove()
    {
        canMove = false;
    }

    private void Update()
    {
        if (!canMove) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTarget();
        }
    }

    private void SetNewTarget()
    {
        float randomX = Random.Range(-moveRangeX, moveRangeX);
        float randomY = Random.Range(-moveRangeY, moveRangeY);

        targetPosition = basePosition + new Vector3(randomX, randomY, 0f);
    }
}