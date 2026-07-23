using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance;

    private void Awake()
    {
        if (
            Instance != null &&
            Instance != this
        )
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool ApplyEffect(
        ItemData item,
        GameObject target
    )
    {
        if (item == null || target == null)
        {
            return false;
        }

        switch (item.itemType)
        {
            case ItemType.Heal:
                return ApplyHeal(item, target);

            case ItemType.Barrier:
                return ApplyBarrier(target);

            case ItemType.SpeedUp:
                return ApplySpeedUp(item, target);

            case ItemType.PaddleExtend:
                return ApplyPaddleExtend(
                    item,
                    target
                );

            case ItemType.AttackUp:
                Debug.Log(
                    "AttackUpは次にスマッシュ倍率へ接続します"
                );

                return true;

            case ItemType.BossStun:
                Debug.Log(
                    "BossStunはBossスクリプトへ接続が必要です"
                );

                return true;
        }

        return false;
    }

    private bool ApplyHeal(
        ItemData item,
        GameObject target
    )
    {
        PlayerHealth health =
            target.GetComponent<PlayerHealth>();

        if (health == null)
        {
            Debug.LogWarning(
                target.name +
                " にPlayerHealthがありません"
            );

            return false;
        }

        health.Heal(item.value);

        Debug.Log(
            target.name +
            " がHPを " +
            item.value +
            " 回復"
        );

        return true;
    }

    private bool ApplyBarrier(
        GameObject target
    )
    {
        PlayerHealth health =
            target.GetComponent<PlayerHealth>();

        if (health == null)
        {
            Debug.LogWarning(
                target.name +
                " にPlayerHealthがありません"
            );

            return false;
        }

        health.GiveShield();

        return true;
    }

    private bool ApplySpeedUp(
        ItemData item,
        GameObject target
    )
    {
        Paddle[] paddles =
            target.GetComponents<Paddle>();

        if (paddles.Length == 0)
        {
            return false;
        }

        foreach (Paddle paddle in paddles)
        {
            paddle.SpeedUp(item.duration);
        }

        Debug.Log(
            target.name +
            " が " +
            item.duration +
            " 秒間スピードアップ"
        );

        return true;
    }

    private bool ApplyPaddleExtend(
        ItemData item,
        GameObject target
    )
    {
        Paddle[] paddles =
            target.GetComponents<Paddle>();

        if (paddles.Length == 0)
        {
            return false;
        }

        // 同じGameObjectに複数のPaddleコンポーネントがあっても、
        // Transformは共通なのでGrowは1回だけでいい
        paddles[0].Grow(item.duration);

        Debug.Log(
            target.name +
            " が " +
            item.duration +
            " 秒間パドル延長"
        );

        return true;
    }
}