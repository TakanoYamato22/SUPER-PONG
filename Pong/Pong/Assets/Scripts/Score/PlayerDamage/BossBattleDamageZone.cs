using UnityEngine;

public class BossBattleDamageZone : MonoBehaviour
{
    [SerializeField] private PlayerHealth targetPlayer;
    [SerializeField] private float gimmickBallDamage = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GimmickBall gimmickBall))
        {
            targetPlayer.TakeDamage(gimmickBallDamage);
            Destroy(gimmickBall.gameObject);
            return;
        }

        if (!collision.TryGetComponent(out Ball ball))
            return;

        targetPlayer.TakeDamage(ball.currentSpeed);

        ball.ResetAndStartWithDelay(1f);
    }
}