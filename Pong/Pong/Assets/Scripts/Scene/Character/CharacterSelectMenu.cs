using UnityEngine;
using UnityEngine.SceneManagement;

// キャラクター選択画面の管理
public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] private CharacterDatabase database;

    // ボタンから呼ばれる
    public void SelectCharacter(int index)
    {
        CharacterData selected = database.GetCharacter(index);

        if (selected == null)
            return;

        // 選択したキャラ番号を保存
        GameSettings.selectedCharacterIndex = index;

        Debug.Log("選択キャラ: " + selected.characterName);

        // ゲームシーンへ移動
        SceneManager.LoadScene("GameScene");
    }

    // 戻るボタン用
    public void BackToStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}