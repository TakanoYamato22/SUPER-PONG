using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public int playerScore { get; private set; }
    public int computerScore { get; private set; }

    public UnityEvent<int> onPlayerScoreChanged;
    public UnityEvent<int> onComputerScoreChanged;

    public UnityEvent onPlayerScored;
    public UnityEvent onComputerScored;

    public void AddPlayerScore()
    {
        playerScore++;
        onPlayerScoreChanged.Invoke(playerScore);
        onPlayerScored.Invoke();
    }

    public void AddComputerScore()
    {
        computerScore++;
        onComputerScoreChanged.Invoke(computerScore);
        onComputerScored.Invoke();
    }

    public void ResetScores()
    {
        playerScore = 0;
        computerScore = 0;

        onPlayerScoreChanged.Invoke(playerScore);
        onComputerScoreChanged.Invoke(computerScore);
    }

    public int GetScoreDifference()
    {
        return playerScore - computerScore;
    }
}
