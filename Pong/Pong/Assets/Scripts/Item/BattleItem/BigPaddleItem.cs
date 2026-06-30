using UnityEngine;

public class BigPaddleItem : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private AudioClip itemSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        Paddle[] paddles = FindObjectsByType<Paddle>(FindObjectsSortMode.None);

        foreach (Paddle paddle in paddles)
        {
            paddle.Grow(duration);
        }

        AudioSource.PlayClipAtPoint(itemSound, transform.position);
        Destroy(gameObject);
    }
}