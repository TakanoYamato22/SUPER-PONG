using UnityEngine;

/// <summary>
/// 壁・Paddleでは通常Ballのように反射する。
/// Bossには速度条件を無視して固定ダメージを与える。
/// </summary>
public class VolcanoStoneBall : MonoBehaviour
{
    [Header("Bossダメージ")]
    [Tooltip("速度に関係なくBossへ与える固定ダメージ")]
    [SerializeField] private float bossDamage = 10f;

    [Header("自動削除")]
    [Tooltip("画面外に残った場合に削除するまでの時間")]
    [Min(0.1f)]
    [SerializeField] private float lifeTime = 10f;

    [Header("Goal設定")]
    [Tooltip("Stoneを削除するGoalのタグ")]
    [SerializeField]
    private string[] destroyGoalTags =
    {
        "PlayerGoal",
        "ComputerGoal"
    };

    private BossController ownerBoss;
    private bool alreadyProcessed;

    private void Start()
    {
        Destroy(
            gameObject,
            lifeTime
        );
    }

    public void Initialize(
        BossController owner,
        float damage
    )
    {
        ownerBoss = owner;
        bossDamage = damage;

        IgnoreOwnerCollision();
    }

    private void IgnoreOwnerCollision()
    {
        if (ownerBoss == null)
        {
            return;
        }

        Collider2D[] stoneColliders =
            GetComponentsInChildren<Collider2D>(
                true
            );

        Collider2D[] ownerColliders =
            ownerBoss.GetComponentsInChildren<Collider2D>(
                true
            );

        foreach (
            Collider2D stoneCollider
            in stoneColliders
        )
        {
            if (stoneCollider == null)
            {
                continue;
            }

            foreach (
                Collider2D ownerCollider
                in ownerColliders
            )
            {
                if (ownerCollider == null)
                {
                    continue;
                }

                Physics2D.IgnoreCollision(
                    stoneCollider,
                    ownerCollider,
                    true
                );
            }
        }
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        if (collision == null)
        {
            return;
        }

        ProcessHit(
            collision.collider
        );
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        ProcessHit(other);
    }

    private void ProcessHit(
        Collider2D other
    )
    {
        if (alreadyProcessed ||
            other == null)
        {
            return;
        }

        // Goalに入ったら削除
        if (IsDestroyGoal(other))
        {
            DestroyStone(
                "Goalに入った"
            );

            return;
        }

        BossController hitBoss =
            other.GetComponentInParent<BossController>();

        if (hitBoss == null)
        {
            // 壁やPaddleでは消さない
            return;
        }

        // 発射元Dragonには当てない
        if (hitBoss == ownerBoss)
        {
            return;
        }

        alreadyProcessed = true;

        // 速度判定を通さず直接ダメージ
        hitBoss.TakeDamage(
            bossDamage
        );

        Debug.Log(
            $"Stone命中：速度を無視して{bossDamage}ダメージ",
            hitBoss
        );

        Destroy(gameObject);
    }

    private bool IsDestroyGoal(
        Collider2D other
    )
    {
        if (destroyGoalTags == null)
        {
            return false;
        }

        foreach (
            string goalTag
            in destroyGoalTags
        )
        {
            if (string.IsNullOrWhiteSpace(
                    goalTag
                ))
            {
                continue;
            }

            if (other.CompareTag(goalTag))
            {
                return true;
            }

            Transform current =
                other.transform.parent;

            while (current != null)
            {
                if (current.CompareTag(goalTag))
                {
                    return true;
                }

                current = current.parent;
            }
        }

        return false;
    }

    private void DestroyStone(
        string reason
    )
    {
        if (alreadyProcessed)
        {
            return;
        }

        alreadyProcessed = true;

        Debug.Log(
            $"Stone削除：{reason}",
            this
        );

        Destroy(gameObject);
    }
}