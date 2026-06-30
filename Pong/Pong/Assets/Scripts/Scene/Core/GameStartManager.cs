using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    [Header("VS用ステージScene")]
    [SerializeField] private string[] vsStageScenes;

    [Header("Boss用ステージScene")]
    [SerializeField] private string[] bossStageScenes;

    public void StartGame()
    {
        string sceneName = GetSelectedSceneName();

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("読み込むScene名が空です");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private string GetSelectedSceneName()
    {
        if (GameSettings.gameMode == GameMode.BOSS)
        {
            return GetSceneFromArray(bossStageScenes);
        }

        return GetSceneFromArray(vsStageScenes);
    }

    private string GetSceneFromArray(string[] scenes)
    {
        if (scenes == null || scenes.Length == 0)
        {
            Debug.LogError("ステージSceneが登録されていません");
            return "";
        }

        if (GameSettings.stageIndex < 0 || GameSettings.stageIndex >= scenes.Length)
        {
            Debug.LogWarning("stageIndexが範囲外です。0番のステージを読み込みます");
            return scenes[0];
        }

        return scenes[GameSettings.stageIndex];
    }
}