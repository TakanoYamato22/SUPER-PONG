using UnityEngine;

public class BossStunItem : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private AudioClip itemSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        GiusController boss = FindFirstObjectByType<GiusController>();

        if (boss != null)
            boss.Stun(duration);

        AudioSource.PlayClipAtPoint(itemSound, transform.position);
        Destroy(gameObject);
    }
}