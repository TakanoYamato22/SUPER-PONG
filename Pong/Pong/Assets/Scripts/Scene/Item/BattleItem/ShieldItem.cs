using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    [SerializeField] private AudioClip itemSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        PlayerHealth[] healths = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        foreach (PlayerHealth health in healths)
        {
            health.GiveShield();
        }

        AudioSource.PlayClipAtPoint(itemSound, transform.position);
        Destroy(gameObject);
    }
}