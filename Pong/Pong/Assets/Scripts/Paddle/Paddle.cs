using UnityEngine;
using System.Collections;

public abstract class Paddle : MonoBehaviour
{
    public float speed = 5f;

    [Header("Smash")]
    public float smashPower = 2f;

    private Vector3 originalScale;
    private Coroutine growCoroutine;
    private Coroutine speedCoroutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, 0f);
    }

    public void Shrink(float duration)
    {
        var size = GetComponent<PaddleSizeController>();
        if (size != null)
            size.Shrink(duration);
    }

    public void Grow(float duration)
    {
        if (growCoroutine != null)
            StopCoroutine(growCoroutine);

        growCoroutine = StartCoroutine(GrowRoutine(duration));
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

    public void SpeedUp(float duration)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(SpeedUpRoutine(duration));
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