using UnityEngine;
using System.Collections;

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyEffect(ItemData item, GameObject target)
    {
        switch (item.itemType)
        {
            case ItemType.AttackUp:
                StartCoroutine(AttackUp(target, item));
                break;

            case ItemType.Heal:
                Heal(target, item);
                break;

            case ItemType.Barrier:
                Barrier(target, item);
                break;

            case ItemType.SpeedUp:
                StartCoroutine(SpeedUp(target, item));
                break;

            case ItemType.BossStun:
                BossStun(item);
                break;

            case ItemType.BossAttackDown:
                BossAttackDown(item);
                break;

            case ItemType.PaddleExtend:
                StartCoroutine(PaddleExtend(target, item));
                break;
        }
    }

    // =========================
    // 攻撃UP
    // =========================

    IEnumerator AttackUp(GameObject target, ItemData item)
    {
        Paddle paddle = target.GetComponent<Paddle>();

        paddle.smashPower += item.value;

        yield return new WaitForSeconds(item.duration);

        paddle.smashPower -= item.value;
    }

    // =========================
    // 回復
    // =========================

    void Heal(GameObject target, ItemData item)
    {
        Debug.Log("HP回復");
    }

    // =========================
    // バリア
    // =========================

    void Barrier(GameObject target, ItemData item)
    {
        Debug.Log("バリア");
    }

    // =========================
    // スピードUP
    // =========================

    IEnumerator SpeedUp(GameObject target, ItemData item)
    {
        Paddle paddle = target.GetComponent<Paddle>();

        paddle.speed += item.value;

        yield return new WaitForSeconds(item.duration);

        paddle.speed -= item.value;
    }

    // =========================
    // ボススタン
    // =========================

    void BossStun(ItemData item)
    {
        Debug.Log("ボススタン");
    }

    // =========================
    // ボス攻撃DOWN
    // =========================

    void BossAttackDown(ItemData item)
    {
        Debug.Log("ボス攻撃ダウン");
    }

    // =========================
    // パドル伸ばす
    // =========================

    IEnumerator PaddleExtend(GameObject target, ItemData item)
    {
        Vector3 original = target.transform.localScale;

        Vector3 extended = original;
        extended.y += item.value;

        target.transform.localScale = extended;

        yield return new WaitForSeconds(item.duration);

        target.transform.localScale = original;
    }
}