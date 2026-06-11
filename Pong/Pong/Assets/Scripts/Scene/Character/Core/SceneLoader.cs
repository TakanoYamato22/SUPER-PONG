using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void GoToItemSelect()
    {
        SceneManager.LoadScene("ItemSelectScene");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void GoToModeSelect()
    {
        SceneManager.LoadScene("ModeSelectScene");
    }
}