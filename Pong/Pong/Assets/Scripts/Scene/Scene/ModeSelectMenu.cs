using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    public void SelectCPU()
    {
        GameSettings.gameMode = GameMode.CPU;
        SceneManager.LoadScene("StageSelectScene");
    }

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
}