using UnityEngine;

// 壁オブジェクト（ボールが当たるとダメージを受ける）
public class WallBlock : MonoBehaviour
{
    [Header("Wall Settings")]

    // 壁の耐久値（何回当たったら壊れるか）
    public int hp = 1;

    // ===============================
    // 💥 ダメージ処理
    // ===============================
    public void TakeDamage()
    {
        hp--;

        Debug.Log("壁にヒット！残りHP: " + hp);

        // HPが0以下なら破壊
        if (hp <= 0)
        {
            Debug.Log("壁破壊！");
            Destroy(gameObject);
        }
    }
}