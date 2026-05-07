using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;

    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        scoreManager.onPlayerScoreChanged.AddListener(score => playerScoreText.text = score.ToString());
        scoreManager.onComputerScoreChanged.AddListener(score => computerScoreText.text = score.ToString());

        scoreManager.onPlayerScored.AddListener(NewRound);
        scoreManager.onComputerScored.AddListener(NewRound);

        NewGame();
    }

    public void NewGame()
    {
        scoreManager.ResetScores();
        NewRound();
    }

    public void NewRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();

        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }
}
