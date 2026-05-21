using UnityEngine;

// すべてのキャラクターをまとめて管理
public class CharacterDatabase : MonoBehaviour
{
    [Header("登録されたキャラクター一覧")]
    public CharacterData[] characters;

    // 指定番号のキャラを取得
    public CharacterData GetCharacter(int index)
    {
        if (index < 0 || index >= characters.Length)
        {
            Debug.LogError("キャラクター番号が範囲外です");
            return null;
        }

        return characters[index];
    }
}