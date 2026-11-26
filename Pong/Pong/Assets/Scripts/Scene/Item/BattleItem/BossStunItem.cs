
using UnityEngine;

public class BossStunItem : MonoBehaviour
{
    [Header("スタン設定")]
    [SerializeField] private float duration = 5f;

    [Header("取得音")]
    [SerializeField] private AudioClip itemSound;

    [SerializeField]
    [Range(0f, 1f)]
    private float soundVolume = 1f;

    private bool hasActivated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasActivated)
            return;

        // Ballかどうか確認
        Ball ball = collision.GetComponent<Ball>();

        if (ball == null)
        {
            ball = collision.GetComponentInParent<Ball>();
        }

        if (ball == null)
            return;

        hasActivated = true;

        bool stunnedBoss = false;

        // ========================================
        // Giusステージ
        // ========================================

        GiusController giusBoss =
            FindFirstObjectByType<GiusController>();

        if (giusBoss != null)
        {
            giusBoss.Stun(duration);
            stunnedBoss = true;
        }

        // ========================================
        // 火山ステージ
        // ========================================

        VolcanoBossController volcanoBoss =
            FindFirstObjectByType<VolcanoBossController>();

        if (volcanoBoss != null)
        {
            volcanoBoss.Stun(duration);
            stunnedBoss = true;
        }

        // ========================================
        // 宇宙ステージ
        // ========================================

        SpaceBossController spaceBoss =
            FindFirstObjectByType<SpaceBossController>();

        if (spaceBoss != null)
        {
            spaceBoss.Stun(duration);
            stunnedBoss = true;

            Debug.Log(
                $"宇宙ボスを{duration}秒スタンさせました！"
            );
        }

        // ========================================
        // アトランティスステージ追加！
        // ========================================

        AtlantisBossController atlantisBoss =
            FindFirstObjectByType<AtlantisBossController>();

        if (atlantisBoss != null)
        {
            atlantisBoss.Stun(duration);
            stunnedBoss = true;

            Debug.Log(
                $"アトランティスボスを{duration}秒スタンさせました！"
            );
        }

        // ========================================
        // スタン対象がいない場合
        // ========================================

        if (!stunnedBoss)
        {
            Debug.LogWarning(
                "スタン対象のBossが見つかりませんでした。",
                this
            );

            hasActivated = false;
            return;
        }

        // ========================================
        // 取得音
        // ========================================

        if (itemSound != null)
        {
            AudioSource.PlayClipAtPoint(
                itemSound,
                transform.position,
                soundVolume
            );
        }

        // アイテムを削除
        Destroy(gameObject);
    }
}
