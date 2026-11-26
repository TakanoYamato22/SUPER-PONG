
using UnityEngine;

public class VolcanoBossBall : MonoBehaviour
{
    [Header("Ball設定")]
    [SerializeField] private Ball ball;

    [Header("Bossへのダメージ条件")]
    [SerializeField] private float minimumDamageSpeed = 25f;

    private void Awake()
    {
        // Ballが同じGameObjectにある場合は自動取得
        if (ball == null)
        {
            ball = GetComponent<Ball>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 親オブジェクトも含めてBossを探す
        BossController boss =
            collision.GetComponentInParent<BossController>();

        // Boss以外に当たった場合は何もしない
        if (boss == null)
        {
            return;
        }

        if (ball == null)
        {
            Debug.LogError("Ballが設定されていません！");
            return;
        }

        // 速度25未満ならダメージを与えない
        if (ball.currentSpeed < minimumDamageSpeed)
        {
            Debug.Log($"速度不足！ 現在速度：{ball.currentSpeed}");
            return;
        }

        // Bossが戦闘中でなければ攻撃しない
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

        // 攻撃前のHPを保存
        float previousHP = boss.CurrentHP;

        // Bossにダメージ
        boss.TakeDamage(damage);

        // 無敵時間などでダメージが入らなかった場合
        if (boss.CurrentHP >= previousHP)
        {
            return;
        }

        // ダメージ成功時だけ攻撃力アップを消費
        if (ball.hasPowerUp)
        {
            ball.hasPowerUp = false;
        }

        // ダメージ成功時だけBallを減速
        ball.SetSpeed(ball.baseSpeed);

        Debug.Log(
            $"Bossに{damage}ダメージ！ " +
            $"残りHP：{boss.CurrentHP}/{boss.MaxHP}"
        );
    }
}
