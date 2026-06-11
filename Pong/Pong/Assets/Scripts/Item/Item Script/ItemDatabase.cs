using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public ItemData[] items;

    public ItemData GetItem(int index)
    {
        return items[index];
    }
}