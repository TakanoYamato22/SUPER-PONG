using UnityEngine;

public class BossBall : MonoBehaviour
{
    [SerializeField] private Ball ball;
    public float speedReduceRate = 0.8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BossController boss))
        {
            boss.TakeDamage(ball.currentSpeed);
            ball.SetSpeed(ball.currentSpeed * speedReduceRate);
        }
    }
}
