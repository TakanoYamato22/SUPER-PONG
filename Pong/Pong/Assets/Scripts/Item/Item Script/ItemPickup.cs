using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPaddle player =
            collision.GetComponent<PlayerPaddle>();

        if (player != null)
        {
            // Œø‰Ê”­“®
            ItemEffectManager.Instance.ApplyEffect(
                itemData,
                player.gameObject
            );

            Destroy(gameObject);
        }
    }
}