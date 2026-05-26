using UnityEngine;

[RequireComponent(typeof(Ball))]
public class BallHitBossHandler : MonoBehaviour
{
    private Ball ball;

    [Range(0f, 1f)]
    public float slowRate = 0.7f; // ボスに当たった時の減速率

    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out BossController boss))
        {
            // ダメージ計算
            float damage = ball.velocity.magnitude;
            boss.TakeDamage(damage);

            // ボール減速（貫通）
            float newSpeed = ball.currentSpeed * slowRate;
            ball.SetSpeed(newSpeed);
        }
    }
}
