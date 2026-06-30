using UnityEngine;

public class PaddleSpeedUpItem : MonoBehaviour
{
    [SerializeField] private float duration = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() == null) return;

        Paddle[] paddles = FindObjectsByType<Paddle>(FindObjectsSortMode.None);

        foreach (Paddle paddle in paddles)
        {
            paddle.SpeedUp(duration);
        }

        Destroy(gameObject);
    }
}