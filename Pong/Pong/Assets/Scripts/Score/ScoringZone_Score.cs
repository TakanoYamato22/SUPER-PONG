using UnityEngine;

public class Score_ScoringZone : MonoBehaviour
{
    public bool isPlayerGoal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Ball ball)) return;

        var score = FindObjectOfType<ScoreManager>();

        if (isPlayerGoal)
            score.AddPlayerScore();
        else
            score.AddComputerScore();

        ball.ResetAndStartWithDelay(1.0f);

    }

}
