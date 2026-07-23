using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    [Min(0.01f)]
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup == null)
        {
            Debug.LogError(
                "ScreenFaderと同じオブジェクトにCanvasGroupがありません。",
                this
            );

            enabled = false;
            return;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeToBlack()
    {
        Debug.Log("暗転開始", this);

        yield return Fade(1f);

        Debug.Log(
            $"暗転完了 Alpha={canvasGroup.alpha}",
            this
        );
    }

    public IEnumerator FadeFromBlack()
    {
        Debug.Log("明転開始", this);

        yield return Fade(0f);

        // 最後に強制的に透明にする
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Debug.Log(
            $"明転完了 Alpha={canvasGroup.alpha}",
            this
        );
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        canvasGroup.blocksRaycasts = true;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float rate = Mathf.Clamp01(
                elapsedTime / fadeDuration
            );

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                rate
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha <= 0f)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }
}