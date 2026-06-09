using UnityEngine;

public class WallBlock : MonoBehaviour
{
    [Header("Wall Type Data (ScriptableObject)")]
    public WallTypeData data;   // ← 壁の種類データを Inspector でセット

    private int hp;

    private void Start()
    {
        // ScriptableObject から設定を読み込む
        hp = data.hp;

        // 色を反映
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = data.color;
        }

        // サイズを反映
        transform.localScale = new Vector3(
            data.size.x,
            data.size.y,
            1f
        );
    }

    // ===============================
    // 💥 ダメージ処理
    // ===============================
    public void TakeDamage()
    {
        hp--;

        Debug.Log($"壁にヒット！残りHP: {hp}");

        if (hp <= 0)
        {
            Debug.Log("壁破壊！");
            Destroy(gameObject);
        }
    }
}
