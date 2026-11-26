
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ForestApple : MonoBehaviour
{
    [Header("プレイヤーHP")]
    [SerializeField]
    private PlayerHealth[] playerHealths;

    [Header("回復設定")]
    [SerializeField, Range(0f, 1f)]
    private float healRatio = 0.25f;

    private bool collected;

    private void Awake()
    {
        // シーン内のPlayerHealthを自動取得
        playerHealths =
            FindObjectsByType<PlayerHealth>(
                FindObjectsSortMode.None
            );

        Debug.Log(
            $"【ForestApple】PlayerHealthを{playerHealths.Length}個取得"
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
        {
            return;
        }

        // Ballが触れた場合のみ取得
        Ball ball = other.GetComponent<Ball>();

        if (ball == null)
        {
            return;
        }

        collected = true;

        foreach (PlayerHealth playerHealth in playerHealths)
        {
            if (playerHealth == null || playerHealth.IsDead)
            {
                continue;
            }

            // それぞれの最大HPの25%を回復
            float healAmount =
                playerHealth.MaxHP * healRatio;

            playerHealth.Heal(healAmount);

            Debug.Log(
                $"{playerHealth.gameObject.name} のHPを" +
                $"{healAmount}回復！"
            );
        }

        Destroy(gameObject);
    }
}
