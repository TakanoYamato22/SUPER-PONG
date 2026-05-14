using UnityEngine;
using UnityEngine.SceneManagement;

// シーン遷移専用クラス
public class SceneLoader : MonoBehaviour
{
    // 指定したシーンへ移動
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // タイトルへ戻る
    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // ゲーム開始
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // ゲーム終了
    public void QuitGame()
    {
        Application.Quit();
    }
}