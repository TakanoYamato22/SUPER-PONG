using System.Collections;
using UnityEngine;

public class SpaceScreenRotator : MonoBehaviour
{
    [Header("回転させるカメラ")]
    [SerializeField] private Camera targetCamera;

    [Header("第1形態の回転時間")]
    [SerializeField, Min(0.01f)] private float rotationDuration = 2f;

    private bool hasRotated;
    private bool isRotating;
    private Coroutine rotationCoroutine;
    private Quaternion originalRotation;
    private float rotationPreviousTimeScale = 1f;

    private void Awake()
    {
        if (targetCamera != null)
            originalRotation = targetCamera.transform.rotation;
    }

    public void StartRotation()
    {
        if (hasRotated || isRotating) return;
        if (targetCamera == null)
        {
            Debug.LogWarning("Main Cameraが設定されていません！", this);
            return;
        }

        rotationCoroutine = StartCoroutine(RotationRoutine());
    }

    private IEnumerator RotationRoutine()
    {
        isRotating = true;
        rotationPreviousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        Quaternion start = targetCamera.transform.rotation;
        Quaternion end = start * Quaternion.Euler(0f, 0f, 180f);
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f,
                Mathf.Clamp01(elapsed / rotationDuration));
            targetCamera.transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }

        targetCamera.transform.rotation = end;
        hasRotated = true;
        isRotating = false;
        rotationCoroutine = null;
        Time.timeScale = rotationPreviousTimeScale;
        Debug.Log("空間反転完了！戦闘再開！");
    }

    // SpacePhaseChangerから呼ぶ。形態変化側がTime.timeScaleを管理する。
    public IEnumerator ReturnToOriginalRotation(float duration)
    {
        if (targetCamera == null) yield break;

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        isRotating = false;
        hasRotated = true;

        Quaternion start = targetCamera.transform.rotation;
        if (Quaternion.Angle(start, originalRotation) < 0.5f)
        {
            targetCamera.transform.rotation = originalRotation;
            yield break;
        }

        isRotating = true;
        duration = Mathf.Max(0.01f, duration);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f,
                Mathf.Clamp01(elapsed / duration));
            targetCamera.transform.rotation = Quaternion.Slerp(start, originalRotation, t);
            yield return null;
        }

        targetCamera.transform.rotation = originalRotation;
        isRotating = false;
        Debug.Log("宇宙ボス：カメラ復帰演出完了！");
    }

    private void OnDisable()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
            Time.timeScale = rotationPreviousTimeScale;
        }
        isRotating = false;
    }
}
