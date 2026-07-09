using UnityEngine;
using System.Collections;

// アイテム効果を実行する管理クラス
public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyEffect(ItemData item, GameObject target)
    {
        if (item == null || target == null)
            return;

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

    private IEnumerator AttackUp(GameObject target, ItemData item)
    {
        Paddle paddle = target.GetComponent<Paddle>();

        if (paddle == null)
            yield break;

        paddle.smashPower += item.value;

        Debug.Log("攻撃UP発動");

        yield return new WaitForSeconds(item.duration);

        paddle.smashPower -= item.value;

        Debug.Log("攻撃UP終了");
    }

    private void Heal(GameObject target, ItemData item)
    {
        Debug.Log("HP回復: " + item.value);
        // HPシステムができたらここに追加
    }

    private void Barrier(GameObject target, ItemData item)
    {
        Debug.Log("バリア発動");
        // バリアシステムができたらここに追加
    }

    private IEnumerator SpeedUp(GameObject target, ItemData item)
    {
        Paddle paddle = target.GetComponent<Paddle>();

        if (paddle == null)
            yield break;

        paddle.speed += item.value;

        Debug.Log("スピードUP発動");

        yield return new WaitForSeconds(item.duration);

        paddle.speed -= item.value;

        Debug.Log("スピードUP終了");
    }

    private void BossStun(ItemData item)
    {
        Debug.Log("ボススタン発動: " + item.duration);
        // BossControllerができたらここに追加
    }

    private void BossAttackDown(ItemData item)
    {
        Debug.Log("ボス攻撃力低下: " + item.value);
        // Boss攻撃処理ができたらここに追加
    }

    private IEnumerator PaddleExtend(GameObject target, ItemData item)
    {
        Vector3 originalScale = target.transform.localScale;

        Vector3 newScale = originalScale;
        newScale.y += item.value;

        target.transform.localScale = newScale;

        Debug.Log("パドル延長発動");

        yield return new WaitForSeconds(item.duration);

        target.transform.localScale = originalScale;

        Debug.Log("パドル延長終了");
    }
}