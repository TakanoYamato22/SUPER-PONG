using System.Collections;
using UnityEngine;

public class BossDamageManager : MonoBehaviour
{
    [Header("被弾設定")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hitFlashTime = 0.3f;
    [SerializeField] private float hitInvincibleTime = 1.5f;

    [Header("通常ダメージEffect")]
    [Tooltip("Hierarchy上の通常ダメージEffectを登録")]
    [SerializeField] private GameObject hitEffectRoot;

    [Header("スマッシュダメージEffect")]
    [Tooltip("Hierarchy上のスマッシュEffectを登録")]
    [SerializeField] private GameObject smashEffectRoot;

    private ParticleSystem[] hitParticles;
    private ParticleSystem[] smashParticles;

    private bool isInvincible;

    public bool IsInvincible => isInvincible;

    private void Awake()
    {
        hitParticles =
            InitializeEffect(
                hitEffectRoot,
                "通常ダメージEffect"
            );

        smashParticles =
            InitializeEffect(
                smashEffectRoot,
                "スマッシュEffect"
            );
    }

    public void PlayHitEffect(MonoBehaviour owner)
    {
        if (owner == null)
        {
            Debug.LogWarning(
                "BossDamageManager：Ownerがnullです。",
                this
            );

            return;
        }

        owner.StartCoroutine(HitFlash());
        owner.StartCoroutine(InvincibleCoroutine());

        PlayParticleEffect();
    }

    public IEnumerator HitFlash()
    {
        if (spriteRenderer == null)
        {
            yield break;
        }

        Color originalColor =
            spriteRenderer.color;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(
            hitFlashTime
        );

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    public IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(
            hitInvincibleTime
        );

        isInvincible = false;
    }

    public IEnumerator StartInvincible(float time)
    {
        isInvincible = true;

        Color originalColor = Color.white;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.yellow;
        }

        yield return new WaitForSeconds(time);

        isInvincible = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private ParticleSystem[] InitializeEffect(
        GameObject effectRoot,
        string effectName
    )
    {
        if (effectRoot == null)
        {
            Debug.LogWarning(
                $"{effectName}が設定されていません。",
                this
            );

            return new ParticleSystem[0];
        }

        effectRoot.SetActive(true);

        ParticleSystem[] particles =
            effectRoot.GetComponentsInChildren<ParticleSystem>(
                true
            );

        if (particles.Length == 0)
        {
            Debug.LogWarning(
                $"{effectName}内にParticle Systemがありません。",
                effectRoot
            );

            return particles;
        }

        foreach (ParticleSystem particle in particles)
        {
            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main =
                particle.main;

            // 時間停止中でも表示可能
            main.useUnscaledTime = true;

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        Debug.Log(
            $"{effectName}初期化：Particle数 {particles.Length}",
            effectRoot
        );

        return particles;
    }

    private void PlayParticleEffect()
    {
        // 現在の仕様を維持
        bool isSmash =
            Input.GetKey(KeyCode.Space);

        if (isSmash)
        {
            PlayParticles(
                smashEffectRoot,
                ref smashParticles,
                "スマッシュダメージEffect"
            );
        }
        else
        {
            PlayParticles(
                hitEffectRoot,
                ref hitParticles,
                "通常ダメージEffect"
            );
        }
    }

    private void PlayParticles(
        GameObject effectRoot,
        ref ParticleSystem[] particles,
        string effectName
    )
    {
        if (effectRoot == null)
        {
            Debug.LogWarning(
                $"{effectName}が設定されていません。",
                this
            );

            return;
        }

        effectRoot.SetActive(true);

        // ボスの中心に移動
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

        if (particles.Length == 0)
        {
            Debug.LogWarning(
                $"{effectName}内にParticle Systemがありません。",
                effectRoot
            );

            return;
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

        Debug.Log(
            $"{effectName}再生：Particle数 {particles.Length}",
            effectRoot
        );
    }

    private void OnDisable()
    {
        StopParticles(hitParticles);
        StopParticles(smashParticles);

        isInvincible = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

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