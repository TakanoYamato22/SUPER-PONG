using UnityEngine;

public class BossBall : MonoBehaviour
{
    [SerializeField] private Ball ball;
    public float speedReduceRate = 0.95f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BossController boss))
        {
            float damage = ball.currentSpeed;

            // 攻撃力アップ
            if (ball.hasPowerUp)
            {
                damage *= ball.powerMultiplier;
                ball.hasPowerUp = false;

                Debug.Log("攻撃力アップ発動！ ダメージ : " + damage);
            }

            boss.TakeDamage(damage);

            ball.SetSpeed(ball.currentSpeed * speedReduceRate);
        }
    }
}