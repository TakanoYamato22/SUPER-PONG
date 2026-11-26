
using System.Collections;
using UnityEngine;

public class Eruption : MonoBehaviour
{
    // ==========================================
    // 噴火時間
    // ==========================================

    [Header("噴火時間")]
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float riseDuration = 3f;
    [SerializeField] private float fallDuration = 2f;

    // ==========================================
    // 噴火の高さ（画像の縦倍率）
    // ==========================================

    [Header("噴火の高さ")]
    [SerializeField] private float warningHeight = 0.15f;
    [SerializeField] private float maximumHeight = 3f;

    // ==========================================
    // 参照
    // ==========================================

    [Header("参照")]
    [SerializeField] private Transform eruptionVisual;
    [SerializeField] private Collider2D eruptionCollider;

    private Vector3 originalScale;

    private void Awake()
    {
        if (eruptionVisual != null)
        {
            originalScale = eruptionVisual.localScale;
        }
    }

    private void Start()
    {
        if (eruptionVisual == null || eruptionCollider == null)
        {
            Debug.LogError(
                "EruptionのVisualまたはColliderが設定されていません！",
                this
            );
            return;
        }

        StartCoroutine(EruptionRoutine());
    }

    private IEnumerator EruptionRoutine()
    {
        // ======================================
        // ① 予告（2秒・当たり判定なし）
        // ======================================

        eruptionCollider.enabled = false;

        SetHeight(warningHeight);

        yield return new WaitForSeconds(warningDuration);

        // ======================================
        // ② 上昇（3秒・当たり判定あり）
        // ======================================

        eruptionCollider.enabled = true;

        float timer = 0f;

        while (timer < riseDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(
                timer / Mathf.Max(0.01f, riseDuration)
            );

            float height = Mathf.Lerp(
                warningHeight,
                maximumHeight,
                t
            );

            SetHeight(height);

            yield return null;
        }

        SetHeight(maximumHeight);

        // ======================================
        // ③ 下降（2秒・当たり判定あり）
        // ======================================

        timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(
                timer / Mathf.Max(0.01f, fallDuration)
            );

            float height = Mathf.Lerp(
                maximumHeight,
                0f,
                t
            );

            SetHeight(height);

            yield return null;
        }

        // ======================================
        // ④ 噴火終了
        // ======================================

        eruptionCollider.enabled = false;

        Destroy(gameObject);
    }

    // ==========================================
    // 炎の高さを変更
    // ==========================================

    private void SetHeight(float height)
    {
        Vector3 scale = originalScale;

        scale.y = originalScale.y * height;

        eruptionVisual.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BallBurn ballBurn = other.GetComponent<BallBurn>();

        if (ballBurn == null)
            return;

        ballBurn.ApplyBurn();
    }

}
