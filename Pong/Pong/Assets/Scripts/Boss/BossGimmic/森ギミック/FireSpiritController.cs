
using System.Collections;
using UnityEngine;

public class FireSpiritController : MonoBehaviour
{
    // ========================================
    // 炎の精霊
    // ========================================

    [Header("炎の精霊")]
    [SerializeField]
    private ForestSpirit fireSpirit;

    // ========================================
    // FireBall
    // ========================================

    [Header("FireBall")]
    [SerializeField]
    private FireSpiritProjectile projectilePrefab;

    [SerializeField]
    private Transform firePoint;

    // ========================================
    // 発射設定
    // ========================================

    [Header("発射設定")]
    [SerializeField, Min(0.1f)]
    private float projectileSpeed = 8f;

    [SerializeField, Min(0f)]
    private float respawnDelay = 2f;

    [SerializeField, Range(0f, 0.95f)]
    private float maxVerticalRatio = 0.6f;

    // ========================================
    // 内部変数
    // ========================================

    private FireSpiritProjectile currentProjectile;
    private Coroutine fireCoroutine;

    // ========================================
    // 初期化
    // ========================================

    private void OnEnable()
    {
        fireCoroutine = StartCoroutine(FireRoutine());
    }

    private void OnDisable()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    // ========================================
    // 発射ループ
    // ========================================

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            if (!CanFire())
            {
                yield return null;
                continue;
            }

            // FireBallが存在する間は待機
            if (currentProjectile != null)
            {
                yield return null;
                continue;
            }

            // 消滅後2秒待つ
            yield return new WaitForSeconds(respawnDelay);

            if (!CanFire() || currentProjectile != null)
            {
                continue;
            }

            Fire();
        }
    }

    // ========================================
    // 発射可能か確認
    // ========================================

    private bool CanFire()
    {
        return fireSpirit != null &&
               !fireSpirit.IsDead &&
               projectilePrefab != null;
    }

    // ========================================
    // FireBall発射
    // ========================================

    private void Fire()
    {
        Vector3 spawnPosition =
            firePoint != null
                ? firePoint.position
                : transform.position;

        // Ball.csと同じ方向決定方式
        float x =
            Random.value < 0.5f
                ? -1f
                : 1f;

        float y = Random.Range(
            -maxVerticalRatio,
            maxVerticalRatio
        );

        Vector2 direction =
            new Vector2(x, y).normalized;

        currentProjectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        currentProjectile.Initialize(
            direction,
            projectileSpeed,
            fireSpirit
        );
    }
}
