using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private Transform wallSpawnPoint;

    private int totalScore = 0;
    private int playerScore;
    private int computerScore;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        }
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        NewRound();
    }

    public void NewRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();

        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
        foreach (var wall in GameObject.FindGameObjectsWithTag("WallBlock"))
        {
            Destroy(wall);
        }
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }

    public void OnPlayerScored()
    {
        SetPlayerScore(playerScore + 1);
        totalScore++;
        CheckPhase();
        NewRound();
    }

    public void OnComputerScored()
    {
        SetComputerScore(computerScore + 1);
        totalScore++;
        CheckPhase();
        NewRound();
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }
    private void CheckPhase()
    {
        if (totalScore == 2)
        {
            SpawnWall();
        }

        if (totalScore >= 5)
        {
            SpawnWall();
        }
    }

    private void SpawnWall()
    {
        Vector2 pos = new Vector2(0, Random.Range(-3f, 3f));
        Instantiate(wallPrefab, pos, Quaternion.identity);
    }

  
    // 🔽 誰のヒットか判定して処理
    public void OnCenterHit(Paddle paddle, int count)
    {
        // プレイヤーのときだけリスク発動
        if (paddle == playerPaddle)
        {
            if (count >= 3)
            {
                playerPaddle.Shrink(3f);
            }
        }

        // CPU側にも将来拡張できる
       // if (paddle == computerPaddle && count >= 3)
       // {
         //   computerPaddle.Shrink(2f);
        //}
    }
}
