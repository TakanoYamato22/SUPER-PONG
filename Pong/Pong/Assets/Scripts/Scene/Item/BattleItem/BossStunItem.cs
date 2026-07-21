using UnityEngine;

public class BossStunItem : MonoBehaviour
{
    [Header("スタン設定")]
    [SerializeField] private float duration = 5f;

    [Header("効果音")]
    [SerializeField] private AudioClip itemSound;

    [SerializeField]
    [Range(0f, 1f)]
    private float soundVolume = 1f;

    private bool hasActivated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 一度だけ発動
        if (hasActivated)
        {
            return;
        }

        // 当たったCollider、または親にBallがあるか確認
        Ball ball = collision.GetComponent<Ball>();

        if (ball == null)
        {
            ball = collision.GetComponentInParent<Ball>();
        }

        // Ball以外では発動しない
        if (ball == null)
        {
            return;
        }

        hasActivated = true;

        bool stunnedBoss = false;

        // Giusがいるステージ
        GiusController giusBoss =
            FindFirstObjectByType<GiusController>();

        if (giusBoss != null)
        {
            giusBoss.Stun(duration);
            stunnedBoss = true;

            Debug.Log(
                $"Giusを{duration}秒スタンさせました。",
                giusBoss
            );
        }

        // VolcanoBossがいるステージ
        VolcanoBossController volcanoBoss =
            FindFirstObjectByType<VolcanoBossController>();

        if (volcanoBoss != null)
        {
            volcanoBoss.Stun(duration);
            stunnedBoss = true;

            Debug.Log(
                $"VolcanoBossを{duration}秒スタンさせました。",
                volcanoBoss
            );
        }

        if (!stunnedBoss)
        {
            Debug.LogWarning(
                "スタン対象のBossが見つかりませんでした。",
                this
            );

            hasActivated = false;
            return;
        }

        // 効果音
        if (itemSound != null)
        {
            AudioSource.PlayClipAtPoint(
                itemSound,
                transform.position,
                soundVolume
            );
        }

        Destroy(gameObject);
    }
}