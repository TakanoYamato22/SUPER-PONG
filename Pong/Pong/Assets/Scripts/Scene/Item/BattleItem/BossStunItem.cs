using UnityEngine;

public class BossStunItem : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        GiusController boss = FindFirstObjectByType<GiusController>();

        if (boss != null)
            boss.Stun(duration);

        Destroy(gameObject);
    }
}