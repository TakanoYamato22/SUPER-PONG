using UnityEngine;

public class WallBlock : MonoBehaviour
{
    public int hp = 3;

    public void TakeDamage()
    {
        hp--;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}