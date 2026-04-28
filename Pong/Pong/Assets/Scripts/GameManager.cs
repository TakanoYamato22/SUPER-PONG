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

    // 壁Prefabと生成位置
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private Transform wallSpawnPoint;

    private int totalScore = 0;
    private int playerScore;
    private int computerScore;

    // ===============================
    // 🔽 壁を出すタイミング管理フラグ
    // ===============================
    private bool shouldSpawnWall = false;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        // Rキーでリセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
    }

    // ===============================
    // 🎮 ゲーム開始
    // ===============================
    public void NewGame()
    {
        totalScore = 0; // 合計スコアもリセット
        SetPlayerScore(0);
        SetComputerScore(0);
        NewRound();
    }

    // ===============================
    // 🔄 ラウンド初期化
    // ===============================
    public void NewRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();

        // 既存の壁を削除（毎ラウンドリセット）
        foreach (var wall in GameObject.FindGameObjectsWithTag("WallBlock"))
        {
            Destroy(wall);
        }

        // 少し待ってからボール発射
        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    // ===============================
    // 🚀 ラウンド開始
    // ===============================
    private void StartRound()
    {
        ball.AddStartingForce();

        // 🔥 ラウンド開始後に壁を出す
        if (shouldSpawnWall)
        {
            SpawnWall();
            shouldSpawnWall = false;
        }
    }

    // ===============================
    // 🟢 プレイヤー得点
    // ===============================
    public void OnPlayerScored()
    {
        SetPlayerScore(playerScore + 1);
        totalScore++;

        CheckPhase(); // フェーズチェック

        NewRound();
    }

    // ===============================
    // 🔴 CPU得点
    // ===============================
    public void OnComputerScored()
    {
        SetComputerScore(computerScore + 1);
        totalScore++;

        CheckPhase(); // フェーズチェック

        NewRound();
    }

    // ===============================
    // 📊 スコア表示更新
    // ===============================
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

    // ===============================
    // 🎯 フェーズ管理（ここで出現条件決める）
    // ===============================
    private void CheckPhase()
    {
        if (totalScore == 2) shouldSpawnWall = true;
        if (totalScore == 5) shouldSpawnWall = true;
        // 例：合計2点で壁出現
        //if (totalScore == 2)
        //{
        //    Debug.Log("壁出現フラグON");
        //    shouldSpawnWall = true;
        //}

        // 他にも増やせる
        // if (totalScore == 5) { shouldSpawnWall = true; }
    }

    // ===============================
    // 🧱 壁生成
    // ===============================
    private void SpawnWall()
    {
        Debug.Log("壁生成！");

        Vector2 pos = new Vector2(0, Random.Range(-2f, 2f));

        Instantiate(wallPrefab, pos, Quaternion.identity);
    }

    // ===============================
    // 🔽 中央ヒットリスク処理
    // ===============================
    public void OnCenterHit(Paddle paddle, int count)
    {
        // プレイヤーだけリスク適用
        if (paddle == playerPaddle)
        {
            if (count >= 3)
            {
                playerPaddle.Shrink(3f);
            }
        }

        // CPUにも適用したいならここ
        // if (paddle == computerPaddle && count >= 3)
        // {
        //     computerPaddle.Shrink(2f);
        // }
    }
}