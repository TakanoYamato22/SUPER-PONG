using UnityEngine;

public class WallDamageDealer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out WallBlock wall))
        {
            wall.TakeDamage();
        }
    }
}
