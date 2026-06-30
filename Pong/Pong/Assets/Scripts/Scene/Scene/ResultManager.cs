using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Text resultText;

    private void Start()
    {
        resultText.text = GameSettings.winnerText;
    }
}