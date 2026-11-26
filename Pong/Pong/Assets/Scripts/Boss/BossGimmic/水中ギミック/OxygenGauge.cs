
using UnityEngine;

public class OxygenGauge : MonoBehaviour
{
    [Header("酸素管理")]
    [SerializeField] private BallOxygenManager oxygenManager;

    [Header("ゲージ")]
    [SerializeField] private SpriteRenderer lightblue;
    [SerializeField] private SpriteRenderer orange;
    [SerializeField] private SpriteRenderer red;
    [SerializeField] private SpriteRenderer black;

    [Header("警告ライン")]
    [SerializeField, Range(0f, 1f)]
    private float warningRatio = 0.7f;

    private SpriteRenderer[] gauges;
    private Vector3[] originalScales;
    private Vector3[] originalPositions;

    private void Awake()
    {
        gauges = new SpriteRenderer[]
        {
            lightblue, orange, red
        };

        originalScales = new Vector3[3];
        originalPositions = new Vector3[3];

        for (int i = 0; i < gauges.Length; i++)
        {
            if (gauges[i] == null)
                continue;

            originalScales[i] =
                gauges[i].transform.localScale;

            originalPositions[i] =
                gauges[i].transform.localPosition;
        }
    }

    private void LateUpdate()
    {
        if (oxygenManager == null)
            return;

        // 第1形態ではゲージを非表示
        bool active = oxygenManager.OxygenEnabled;

        if (black != null)
            black.enabled = active;

        if (!active)
        {
            foreach (SpriteRenderer gauge in gauges)
            {
                if (gauge != null)
                    gauge.enabled = false;
            }

            return;
        }

        float ratio = Mathf.Clamp01(
            oxygenManager.OxygenRatio
        );

        // ゲージの色を選択
        int activeIndex = 0;

        if (ratio >= 1f)
        {
            activeIndex = 2;
        }
        else if (ratio >= warningRatio)
        {
            activeIndex = 1;
        }

        for (int i = 0; i < gauges.Length; i++)
        {
            SpriteRenderer gauge = gauges[i];

            if (gauge == null)
                continue;

            // 使用する色だけ表示
            gauge.enabled = (i == activeIndex);

            if (i != activeIndex)
                continue;

            Transform gaugeTransform = gauge.transform;

            Vector3 scale = originalScales[i];
            Vector3 position = originalPositions[i];

            // 横方向に伸ばす
            scale.x = originalScales[i].x * ratio;

            // 左端を固定するため位置を調整
            if (gauge.sprite != null)
            {
                float width =
                    gauge.sprite.bounds.size.x *
                    originalScales[i].x;

                position.x =
                    originalPositions[i].x -
                    width * (1f - ratio) * 0.5f;
            }

            gaugeTransform.localScale = scale;
            gaugeTransform.localPosition = position;
        }
    }
}
