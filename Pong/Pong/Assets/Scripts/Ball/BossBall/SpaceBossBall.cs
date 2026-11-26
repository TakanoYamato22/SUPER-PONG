
using UnityEngine;

public class SpaceBossBall : MonoBehaviour
{
    [SerializeField] private Ball ball;

    [Header("攻撃後の速度倍率")]
    [SerializeField] private float speedReduceRate = 0.95f;

    private void Awake()
    {
        if (ball == null)
        {
            ball = GetComponent<Ball>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossController boss =
            collision.GetComponentInParent<BossController>();

        if (boss == null || ball == null)
            return;

        if (!boss.BattleStarted || boss.IsDead ||
            boss.IsPhaseChanging)
            return;

        // バリアのColliderに当たった場合は
        // ダメージ処理を行わない
        if (collision.GetComponentInParent<SpaceBossBarrier>() != null)
            return;

        // ボス本体に当たった場合のみダメージ判定
        SpaceBossBarrier barrier =
            boss.GetComponentInChildren<SpaceBossBarrier>();

        if (barrier != null && barrier.IsBarrierActive)
        {
            Debug.Log("バリア展開中！ボスへのダメージ無効！");
            return;
        }

        float damage = ball.currentSpeed;

        if (ball.hasPowerUp)
        {
            damage *= ball.powerMultiplier;
        }

        float previousHP = boss.CurrentHP;

        boss.TakeDamage(damage);

        // 無敵時間などでダメージが通らなかった場合
        if (boss.CurrentHP >= previousHP)
            return;

        if (ball.hasPowerUp)
        {
            ball.hasPowerUp = false;
        }

        ball.SetSpeed(
            ball.currentSpeed * speedReduceRate
        );

        Debug.Log(
            $"宇宙ボスに{damage}ダメージ！" +
            $"残りHP：{boss.CurrentHP}/{boss.MaxHP}"
        );
    }
}
