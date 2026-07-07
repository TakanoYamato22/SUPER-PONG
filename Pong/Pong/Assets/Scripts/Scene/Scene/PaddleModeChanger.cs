using UnityEngine;

public class PaddleModeChanger : MonoBehaviour
{
    [SerializeField] private ComputerPaddle computerPaddle;
    [SerializeField] private ComputerSmashController computerSmashController;

    [SerializeField] private Player2Paddle player2Paddle;
    [SerializeField] private Player2SmashController player2SmashController;

    private void Awake()
    {
        bool isTwoPlayer = GameSettings.playerCount == 2;

        computerPaddle.enabled = !isTwoPlayer;
        computerSmashController.enabled = !isTwoPlayer;

        player2Paddle.enabled = isTwoPlayer;
        player2SmashController.enabled = isTwoPlayer;

        Debug.Log("PlayerCount = " + GameSettings.playerCount);
    }
}