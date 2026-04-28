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

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        // スコアイベント登録
        scoreManager.onPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
        scoreManager.onComputerScoreChanged.AddListener(OnComputerScoreChanged);

        scoreManager.onPlayerScored.AddListener(OnPlayerScored);
        scoreManager.onComputerScored.AddListener(OnComputerScored);

        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
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

        // 壁を全部消す
        foreach (var wall in GameObject.FindGameObjectsWithTag("WallBlock"))
        {
            Destroy(wall);
        }
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }

    // UI 更新 ＋ 壁生成処理
    private void OnPlayerScoreChanged(int score)
    {
        playerScoreText.text = score.ToString();

        // ★ スコアに応じて出す個数を決める ★
        int spawnCount = 0;

        if (score == 3)
            spawnCount = 1;
        else if (score == 5)
            spawnCount = 2;
        else if (score == 7)
            spawnCount = 3;

        if (spawnCount > 0)
            SpawnMultipleWalls(spawnCount);
    }

    private void OnComputerScoreChanged(int score)
    {
        computerScoreText.text = score.ToString();
    }

    private void OnPlayerScored()
    {
        NewRound();
    }

    private void OnComputerScored()
    {
        NewRound();
    }

    // ★ 複数壁生成 ★
    private void SpawnMultipleWalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(wallPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    // ★ ランダム位置生成（画面内） ★
    private Vector2 GetRandomPosition()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float x = Random.Range(-camWidth + 0.5f, camWidth - 0.5f);
        float y = Random.Range(-camHeight + 0.5f, camHeight - 0.5f);

        return new Vector2(x, y);
    }

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
    }
}
