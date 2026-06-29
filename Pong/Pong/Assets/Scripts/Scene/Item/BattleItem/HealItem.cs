using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private float healAmount = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        PlayerHealth[] healths = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        foreach (PlayerHealth health in healths)
        {
            health.Heal(healAmount);
        }

        Destroy(gameObject);
    }
}