using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [Header("登録アイテム一覧")]
    [SerializeField] private ItemData[] items;

    public int ItemCount
    {
        get
        {
            return items == null ? 0 : items.Length;
        }
    }

    public ItemData GetItem(int index)
    {
        if (items == null || items.Length == 0)
        {
            Debug.LogError(
                gameObject.name +
                " のItemDatabaseにアイテムが登録されていません"
            );

            return null;
        }

        if (index < 0 || index >= items.Length)
        {
            Debug.LogError(
                "アイテム番号が範囲外です: " +
                index
            );

            return null;
        }

        return items[index];
    }
}