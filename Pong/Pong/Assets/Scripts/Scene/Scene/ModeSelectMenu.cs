using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    public void SelectVS()
    {
        GameSettings.gameMode = GameMode.VS;
        SceneManager.LoadScene("StageSelectScene");
    }

    public void SelectBoss()
    {
        GameSettings.gameMode = GameMode.BOSS;
        SceneManager.LoadScene("StageSelectScene");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}