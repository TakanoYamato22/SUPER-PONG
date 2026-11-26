
using System.Collections;
using UnityEngine;

public class SpiritDamageManager : MonoBehaviour
{
    // ========================================
    // 被弾設定
    // ========================================

    [Header("被弾設定")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float hitFlashTime = 0.3f;

    [SerializeField]
    private float hitInvincibleTime = 1.5f;

    // ========================================
    // Effect設定
    // ========================================

    [Header("通常ダメージEffect")]
    [SerializeField]
    private GameObject hitEffectRoot;

    [Header("スマッシュダメージEffect")]
    [SerializeField]
    private GameObject smashEffectRoot;

    // ========================================
    // 内部変数
    // ========================================

    private bool isInvincible;
    private Color originalColor;

    private ParticleSystem[] hitParticles;
    private ParticleSystem[] smashParticles;

    public bool IsInvincible => isInvincible;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        hitParticles = InitializeEffect(
            hitEffectRoot
        );

        smashParticles = InitializeEffect(
            smashEffectRoot
        );
    }

    // ========================================
    // 被弾処理
    // ========================================

    public void PlayHitEffect()
    {
        if (isInvincible)
        {
            return;
        }

        // 赤点滅と無敵時間
        StartCoroutine(HitRoutine());

        // 被弾Effect再生
        PlayParticleEffect();
    }

    // ========================================
    // 赤点滅・無敵時間
    // ========================================

    private IEnumerator HitRoutine()
    {
        isInvincible = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        yield return new WaitForSeconds(
            hitFlashTime
        );

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        yield return new WaitForSeconds(
            Mathf.Max(
                0f,
                hitInvincibleTime - hitFlashTime
            )
        );

        isInvincible = false;
    }

    // ========================================
    // Effect初期化
    // ========================================

    private ParticleSystem[] InitializeEffect(
        GameObject effectRoot
    )
    {
        if (effectRoot == null)
        {
            return new ParticleSystem[0];
        }

        effectRoot.SetActive(true);

        ParticleSystem[] particles =
            effectRoot.GetComponentsInChildren<ParticleSystem>(
                true
            );

        foreach (ParticleSystem particle in particles)
        {
            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main =
                particle.main;

            main.useUnscaledTime = true;

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        return particles;
    }

    // ========================================
    // Effectの種類を判定
    // ========================================

    private void PlayParticleEffect()
    {
        // 既存のBossDamageManagerと同じ判定
        bool isSmash = Input.GetKey(KeyCode.Space);

        if (isSmash)
        {
            PlayParticles(
                smashEffectRoot,
                ref smashParticles
            );
        }
        else
        {
            PlayParticles(
                hitEffectRoot,
                ref hitParticles
            );
        }
    }

    // ========================================
    // Particle再生
    // ========================================

    private void PlayParticles(
        GameObject effectRoot,
        ref ParticleSystem[] particles
    )
    {
        if (effectRoot == null)
        {
            return;
        }

        effectRoot.SetActive(true);

        // 精霊の位置にEffectを移動
        effectRoot.transform.position =
            transform.position;

        if (particles == null ||
            particles.Length == 0)
        {
            particles =
                effectRoot.GetComponentsInChildren<ParticleSystem>(
                    true
                );
        }

        foreach (ParticleSystem particle in particles)
        {
            if (particle == null)
            {
                continue;
            }

            particle.gameObject.SetActive(true);

            ParticleSystem.MainModule main =
                particle.main;

            main.useUnscaledTime = true;

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            particle.Clear(true);
            particle.Play(true);
        }
    }

    // ========================================
    // 無効化時の処理
    // ========================================

    private void OnDisable()
    {
        StopAllCoroutines();

        StopParticles(hitParticles);
        StopParticles(smashParticles);

        isInvincible = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    // ========================================
    // Particle停止
    // ========================================

    private void StopParticles(
        ParticleSystem[] particles
    )
    {
        if (particles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in particles)
        {
            if (particle == null)
            {
                continue;
            }

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }
    }
}
