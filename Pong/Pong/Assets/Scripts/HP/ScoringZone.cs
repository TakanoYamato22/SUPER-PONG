//using UnityEngine;


public class ScoringZone : MonoBehaviour
{
    public bool isPlayerGoal;

    public HealthManager playerHealth;
    public HealthManager bossHealth;





        if (isPlayerGoal)
        {
            playerHealth.TakeDamage(ball.velocity.magnitude);
        }
        else
        {
            bossHealth.TakeDamage(ball.velocity.magnitude);
        }

        ball.ResetAndStartWithDelay(1.0f);
    }
}

