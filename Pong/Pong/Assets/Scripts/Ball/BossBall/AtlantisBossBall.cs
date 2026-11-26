
using UnityEngine;

public class AtlantisBossBall : MonoBehaviour
{
    [Header("Ball設定")]
    [SerializeField] private Ball ball;

    [Header("Bossへのダメージ")]
    [SerializeField] private float speedReduceRate = 0.95f;

    private void Awake()
    {
        if (ball == null)
            ball = GetComponent<Ball>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 親オブジェクトも含めてBossを探す
        BossController boss =
            collision.GetComponentInParent<BossController>();

        if (boss == null || ball == null)
            return;

        // 戦闘中のみダメージ
        if (!boss.BattleStarted ||
            boss.IsDead ||
            boss.IsPhaseChanging)
        {
            return;
        }

        float damage = ball.currentSpeed;

        // 攻撃力アップ
        if (ball.hasPowerUp)
        {
            damage *= ball.powerMultiplier;
        }

        float previousHP = boss.CurrentHP;

        // Bossにダメージ
        boss.TakeDamage(damage);

        // 無敵状態などでダメージが入らなかった場合
        if (boss.CurrentHP >= previousHP)
            return;

        // 攻撃力アップを消費
        if (ball.hasPowerUp)
        {
            ball.hasPowerUp = false;
        }

        // ダメージ成功時にBallを減速
        ball.SetSpeed(ball.currentSpeed * speedReduceRate);

        Debug.Log(
            $"Atlantis Bossに{damage}ダメージ！ " +
            $"残りHP：{boss.CurrentHP}/{boss.MaxHP}"
        );
    }
}
