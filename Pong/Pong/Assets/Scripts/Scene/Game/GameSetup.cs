using UnityEngine;

// ゲーム開始時に設定を反映
public class GameSetup : MonoBehaviour
{
    [SerializeField] private GameObject player2Paddle;
    [SerializeField] private GameObject cpuPaddle;
    [SerializeField] private GameObject bossController;

    private void Start()
    {
        // 2PモードならCPUを消して2Pを有効化
        if (GameSettings.playerCount == 2)
        {
            cpuPaddle.SetActive(false);
            player2Paddle.SetActive(true);
        }

        // CPU戦
        if (GameSettings.gameMode == "CPU")
        {
            cpuPaddle.SetActive(true);
        }

        // ボス戦
        if (GameSettings.gameMode == "BOSS")
        {
            if (bossController != null)
            {
                bossController.SetActive(true);
            }
        }

        // ステージ別設定
        switch (GameSettings.stageIndex)
        {
            case 1:
                Debug.Log("Stage 1");
                break;
            case 2:
                Debug.Log("Stage 2");
                break;
            case 3:
                Debug.Log("Stage 3");
                break;
        }
    }
}