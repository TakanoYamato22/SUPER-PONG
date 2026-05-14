using UnityEngine;

// ステージ選択
public class StageSelectMenu : MonoBehaviour
{
    public void SelectStage(int stageNumber)
    {
        GameSettings.stageIndex = stageNumber;
    }
}