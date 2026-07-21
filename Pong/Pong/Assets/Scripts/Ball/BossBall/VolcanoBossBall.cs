using UnityEngine;

public class VolcanoBossBall : MonoBehaviour
{
    [SerializeField] private Ball ball;

    [SerializeField] private float minimumDamageSpeed = 25f; // ダメージを与えられる最低速度

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out BossController boss))
            return;

        // 速度が25未満ならダメージを与えない
        if (ball.currentSpeed < minimumDamageSpeed)
        {
            Debug.Log($"速度不足！ 現在速度：{ball.currentSpeed}");
            return;
        }

        float damage = ball.currentSpeed;

        // 攻撃力アップ
        if (ball.hasPowerUp)
        {
            damage *= ball.powerMultiplier;
            ball.hasPowerUp = false;

            Debug.Log("攻撃力アップ発動！ ダメージ : " + damage);
        }

        // ボスにダメージ
        boss.TakeDamage(damage);

        // ダメージを与えたら速度をBaseSpeedに戻す
        ball.SetSpeed(ball.baseSpeed);

        Debug.Log($"ボスに {damage} ダメージ！ スピードをBaseSpeed({ball.baseSpeed})に戻しました。");
    }
}