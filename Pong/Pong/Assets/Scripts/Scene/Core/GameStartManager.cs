using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    [Header("CPU / VS 共通ステージScene")]
    [SerializeField] private string[] normalStageScenes;

    [Header("Boss専用ステージScene")]
    [SerializeField] private string[] bossStageScenes;

    public void StartGame()
    {
        string sceneName = GetSelectedStageSceneName();

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("読み込むScene名が空です");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private string GetSelectedStageSceneName()
    {
        if (GameSettings.gameMode == GameMode.BOSS)
        {
            return GetSceneNameFromArray(bossStageScenes);
        }

        return GetSceneNameFromArray(normalStageScenes);
    }

    private string GetSceneNameFromArray(string[] scenes)
    {
        if (scenes == null || scenes.Length == 0)
        {
            Debug.LogError("ステージScene配列が空です");
            return "";
        }

        int index = GameSettings.stageIndex;

        if (index < 0 || index >= scenes.Length)
        {
            Debug.LogWarning("stageIndexが範囲外なので0番を読み込みます: " + index);
            index = 0;
        }

        return scenes[index];
    }
}