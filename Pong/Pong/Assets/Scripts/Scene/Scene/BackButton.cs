using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void BackToModeSelect()
    {
        SceneManager.LoadScene("ModeSelectScene");
    }

    public void BackToStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void BackToCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void BackToItemSelect()
    {
        SceneManager.LoadScene("ItemSelectScene");
    }
}