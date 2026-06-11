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
        GameSettings.player1CharacterIndex = index;

        Debug.Log("選択キャラ: " + selected.characterName);

        // アイテムシーンへ移動
        SceneManager.LoadScene("ItemSelectScene");
    }

    // 戻るボタン用
    public void BackToStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}