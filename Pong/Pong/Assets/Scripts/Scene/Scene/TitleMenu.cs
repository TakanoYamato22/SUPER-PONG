using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void Select1P()
    {
        GameSettings.playerCount = 1;
        SceneManager.LoadScene("ModeSelectScene");
    }

    public void Select2P()
    {
        GameSettings.playerCount = 2;
        SceneManager.LoadScene("ModeSelectScene");
    }
}