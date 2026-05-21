using UnityEngine;
using System.Collections;

public class PaddleSizeController : MonoBehaviour
{
    private bool isShrinking = false;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Shrink(float duration)
    {
        if (!isShrinking)
            StartCoroutine(ShrinkRoutine(duration));
    }

    private IEnumerator ShrinkRoutine(float duration)
    {
        isShrinking = true;

        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * 0.6f,
            originalScale.z
        );

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
        isShrinking = false;
    }
}
