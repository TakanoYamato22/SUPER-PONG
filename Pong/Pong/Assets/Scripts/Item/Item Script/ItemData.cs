using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Pong/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("基本情報")]
    public string itemName;
    public Sprite icon;

    [Header("アイテム種類")]
    public ItemType itemType;

    [Header("数値")]
    public float value = 1f;

    [Header("効果時間")]
    public float duration = 5f;
}