using UnityEngine;

public class PaddleModeChanger : MonoBehaviour
{
    [Header("1人プレイ用")]
    [SerializeField] private ComputerPaddle computerPaddle;

    [Header("2人プレイ用")]
    [SerializeField] private Player2Paddle player2Paddle;
    [SerializeField] private SmashController player2SmashController;

    private void Awake()
    {
        ChangeMode();
    }

    public void ChangeMode()
    {
        bool isTwoPlayer = GameSettings.playerCount == 2;

        // 1人ならCPUを有効
        if (computerPaddle != null)
        {
            computerPaddle.enabled = !isTwoPlayer;
        }

        // 2人なら2P操作を有効
        if (player2Paddle != null)
        {
            player2Paddle.enabled = isTwoPlayer;
        }

        if (player2SmashController != null)
        {
            player2SmashController.enabled = isTwoPlayer;
        }

    }
}