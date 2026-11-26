using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [Header("“o˜^ƒAƒCƒeƒ€ˆê——")]
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
                " ‚ÌItemDatabase‚ÉƒAƒCƒeƒ€‚ª“o˜^‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ"
            );

            return null;
        }

        if (index < 0 || index >= items.Length)
        {
           
            

            return null;
        }

        return items[index];
    }
}