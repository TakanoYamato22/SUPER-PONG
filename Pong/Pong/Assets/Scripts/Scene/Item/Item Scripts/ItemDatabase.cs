using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [Header("登録アイテム一覧")]
    public ItemData[] items;

    public ItemData GetItem(int index)
    {
        if (index < 0 || index >= items.Length)
        {
            Debug.LogError("アイテム番号が範囲外です: " + index);
            return null;
        }

        return items[index];
    }
}