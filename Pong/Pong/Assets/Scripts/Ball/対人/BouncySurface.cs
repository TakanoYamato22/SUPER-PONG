using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BouncySurface : MonoBehaviour
{
    public enum ForceType
    {
        Additive,
        Multiplicative,
    }

    public ForceType forceType = ForceType.Additive;
    public float bounceStrength = 0f;

    [SerializeField] private PlayerSmashController playerSmashController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            if (playerSmashController != null && playerSmashController.CanSmashNow())
            {
                playerSmashController.DoSmash();
                return;
            }

            switch (forceType)
            {
                case ForceType.Additive:
                    ball.IncreaseSpeed(bounceStrength);
                    return;

                case ForceType.Multiplicative:
                    float multiplier = bounceStrength;
                    float addAmount = ball.currentSpeed * (multiplier - 1f);
                    ball.IncreaseSpeed(addAmount);
                    return;
            }
        }
    }
}