using UnityEngine;

public class PaddleModeChanger : MonoBehaviour
{
   
    [SerializeField] private Player2Paddle player2Paddle;
    [SerializeField] private Player2SmashController player2SmashController;

    private void Awake()
    {
        bool isTwoPlayer = GameSettings.playerCount == 2;

        player2Paddle.enabled = isTwoPlayer;
        player2SmashController.enabled = isTwoPlayer;

        Debug.Log("PlayerCount = " + GameSettings.playerCount);
    }
}