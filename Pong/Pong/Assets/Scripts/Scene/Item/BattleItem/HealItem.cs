using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private float healAmount = 15f;
    [SerializeField] private AudioClip itemSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        PlayerHealth[] healths = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        foreach (PlayerHealth health in healths)
        {
            health.Heal(healAmount);
        }

        AudioSource.PlayClipAtPoint(itemSound, transform.position);
        Destroy(gameObject);
    }
}