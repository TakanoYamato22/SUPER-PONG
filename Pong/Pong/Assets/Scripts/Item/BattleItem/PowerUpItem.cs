using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [SerializeField] private float multiplier = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.GetComponent<Ball>();

        if (ball == null) return;

        ball.GivePowerUp(multiplier);

        Destroy(gameObject);
    }
}