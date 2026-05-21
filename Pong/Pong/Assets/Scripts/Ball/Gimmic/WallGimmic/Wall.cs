using UnityEngine;

public class Wall : MonoBehaviour
{
    public WallTypeData data;
    private int hp;

    private void Awake()
    {
        hp = data.hp; // ScriptableObject から初期値をコピー
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
