
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FireSpiritGoal : MonoBehaviour
{
    [Header("ダメージを受けるプレイヤー")]
    [SerializeField]
    private PlayerHealth targetPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FireSpiritProjectile projectile =
            other.GetComponent<FireSpiritProjectile>();

        if (projectile == null)
        {
            return;
        }

        projectile.HitGoal(targetPlayer);
    }
}
