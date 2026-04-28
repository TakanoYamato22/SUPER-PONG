using UnityEngine;

public class ScoringZone : MonoBehaviour
{
    public bool isPlayerGoal; // true = プレイヤー得点, false = CPU得点

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Ball _)) return;

        var score = FindObjectOfType<ScoreManager>();

        if (isPlayerGoal)
            score.AddPlayerScore();
        else
            score.AddComputerScore();
    }
}
