using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectMenu : MonoBehaviour
{
    public void SelectStage(int stageNumber)
    {
        GameSettings.stageIndex = stageNumber;

        Debug.Log("選択ステージ: " + stageNumber);

        SceneManager.LoadScene("CharacterSelectScene");
    }
}