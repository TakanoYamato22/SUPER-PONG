
using System.Collections;
using UnityEngine;

public class SpaceBossBarrier : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private SpaceBossController boss;
    [SerializeField] private GameObject barrierVisual;
    [SerializeField] private Collider2D barrierCollider;

    [Header("バリア設定")]
    [SerializeField, Range(0f, 1f)]
    private float activateHpRatio = 0.3f;

    [SerializeField] private float respawnTime = 10f;

    private bool unlocked;
    private bool barrierActive;
    private Coroutine respawnCoroutine;

    public bool IsBarrierActive => barrierActive;

    private void Start()
    {
        if (boss == null)
            boss = GetComponentInParent<SpaceBossController>();

        if (barrierCollider == null)
            barrierCollider = GetComponent<Collider2D>();

        SetBarrierActive(false);
    }

    private void Update()
    {
        if (boss == null || boss.IsDead)
            return;

        if (!unlocked &&
            boss.BattleStarted &&
            boss.MaxHP > 0f &&
            boss.CurrentHP / boss.MaxHP <= activateHpRatio)
        {
            unlocked = true;
            SetBarrierActive(true);
        }
    }

    public void BreakBarrier()
    {
        if (!barrierActive)
            return;

        SetBarrierActive(false);

        Debug.Log("宇宙ボスのバリア破壊！");

        if (respawnCoroutine != null)
            StopCoroutine(respawnCoroutine);

        respawnCoroutine = StartCoroutine(RespawnBarrier());
    }

    private IEnumerator RespawnBarrier()
    {
        yield return new WaitForSeconds(respawnTime);

        if (boss != null && !boss.IsDead)
        {
            SetBarrierActive(true);
            Debug.Log("宇宙ボスのバリア復活！");
        }

        respawnCoroutine = null;
    }

    private void SetBarrierActive(bool active)
    {
        barrierActive = active;

        if (barrierVisual != null)
            barrierVisual.SetActive(active);

        if (barrierCollider != null)
            barrierCollider.enabled = active;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!barrierActive)
            return;

        BallSmashManager smash =
            collision.GetComponentInParent<BallSmashManager>();

        if (smash == null)
            return;

        if (smash.IsSmashed)
        {
            // スマッシュならバリア破壊
            BreakBarrier();

            Debug.Log("スマッシュBallがバリアを貫通！");
        }
        else
        {
            // 通常Ballは反射
            Rigidbody2D rb =
                collision.attachedRigidbody;

            if (rb == null)
                return;

            Vector2 normal =
                ((Vector2)rb.position -
                 (Vector2)transform.position).normalized;

            if (normal == Vector2.zero)
                normal = -rb.linearVelocity.normalized;

            Vector2 velocity = rb.linearVelocity;

            // バリアに向かっているときだけ反射
            if (Vector2.Dot(velocity, normal) < 0f)
            {
                rb.linearVelocity =
                    Vector2.Reflect(velocity, normal);

                Debug.Log("通常Ballをバリアで反射！");
            }
        }
    }

    private void OnDisable()
    {
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }
}
