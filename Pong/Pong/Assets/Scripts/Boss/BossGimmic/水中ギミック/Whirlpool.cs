
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    [Header("回転設定")]
    [SerializeField] private float rotationSpeed = 90f;

    [Header("引力設定")]
    [SerializeField, Min(0.1f)]
    private float attractionRadius = 3f;

    [Tooltip("Ballの進行方向を曲げる強さ")]
    [SerializeField, Min(0f)]
    private float attractionStrength = 2f;

    [Header("存在時間")]
    [SerializeField, Min(0.1f)]
    private float lifetime = 8f;

    private float lifeTimer;

    private static readonly List<Whirlpool> activeWhirlpools =
        new List<Whirlpool>();

    private void OnEnable()
    {
        if (!activeWhirlpools.Contains(this))
        {
            activeWhirlpools.Add(this);
        }

        lifeTimer = 0f;
    }

    private void OnDisable()
    {
        activeWhirlpools.Remove(this);
    }

    private void Update()
    {
        // 渦潮を回転
        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime
        );

        // 一定時間後に消滅
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    // ========================================
    // Ballに作用する渦潮の引力を計算
    // ========================================

    public static Vector2 GetCombinedPull(Vector2 ballPosition)
    {
        Vector2 totalPull = Vector2.zero;

        foreach (Whirlpool whirlpool in activeWhirlpools)
        {
            if (whirlpool == null ||
                !whirlpool.isActiveAndEnabled)
            {
                continue;
            }

            Vector2 direction =
                (Vector2)whirlpool.transform.position -
                ballPosition;

            float distance = direction.magnitude;

            // 範囲外、または中心に重なっている場合
            if (distance >= whirlpool.attractionRadius ||
                distance < 0.001f)
            {
                continue;
            }

            // 中心に近いほど引力が強くなる
            float strengthRatio =
                1f - distance / whirlpool.attractionRadius;

            float pullStrength =
                whirlpool.attractionStrength *
                strengthRatio;

            totalPull +=
                direction.normalized * pullStrength;
        }

        return totalPull;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            attractionRadius
        );
    }
#endif
}
