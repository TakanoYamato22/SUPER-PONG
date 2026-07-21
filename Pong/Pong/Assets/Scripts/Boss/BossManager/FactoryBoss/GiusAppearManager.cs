using System.Collections;
using UnityEngine;

public class GiusAppearManager : MonoBehaviour
{
    [Header("ボス設定")]
    [SerializeField] private Transform boss;
    [SerializeField] private BossController bossController;
    [SerializeField] private GiusController giusController;

    [Header("登場位置")]
    [SerializeField] private Vector3 startPosition;

    [Tooltip("登場後に止まる位置。Yを2にしたい場合はYを2に設定")]
    [SerializeField]
    private Vector3 targetPosition =
        new Vector3(0f, 2f, 0f);

    [Header("登場移動")]
    [Tooltip("開始位置から登場位置まで移動する時間")]
    [SerializeField] private float appearTime = 1.5f;

    [Header("登場完了Effect")]
    [Tooltip("Hierarchy上に置いた登場Effectのルート")]
    [SerializeField] private GameObject appearEffectRoot;

    [Tooltip("Effect再生後、移動開始まで待つ時間")]
    [SerializeField] private float effectWaitTime = 0.8f;

    [Header("効果音")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip appearSE;

    private ParticleSystem[] appearParticles;

    private Coroutine appearCoroutine;

    private float savedTimeScale = 1f;
    private bool appearing;

    private void Awake()
    {
        InitializeAppearEffect();
    }

    private void Start()
    {
        appearCoroutine =
            StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        if (boss == null)
        {
            Debug.LogWarning(
                "GiusAppearManager：Bossが設定されていません！",
                this
            );

            yield break;
        }

        appearing = true;

        savedTimeScale = Time.timeScale;

        // Gius以外の時間を停止
        Time.timeScale = 0f;

        // 登場中は戦闘処理を停止
        if (bossController != null)
        {
            bossController.SetBattleStarted(false);
        }

        boss.position = startPosition;

        if (audioSource != null &&
            appearSE != null)
        {
            audioSource.PlayOneShot(appearSE);
        }

        float timer = 0f;
        float safeAppearTime =
            Mathf.Max(0.01f, appearTime);

        // 開始位置からtargetPositionまで登場
        while (timer < safeAppearTime)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                timer / safeAppearTime
            );

            float smoothT =
                Mathf.SmoothStep(0f, 1f, t);

            boss.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                smoothT
            );

            yield return null;
        }

        // 必ず目的地に合わせる
        boss.position = targetPosition;

        Debug.Log(
            $"Gius：登場位置に到着 Y={boss.position.y}",
            this
        );

        // Y=2付近に到着したあとEffect再生
        PlayAppearEffect();

        // TimeScaleが0でも待てる
        yield return new WaitForSecondsRealtime(
            effectWaitTime
        );

        StopAppearEffect();

        // Effect終了後にゲーム全体を再開
        RestoreTimeScale();

        appearing = false;

        // Effectが終わってから移動開始
        if (giusController != null)
        {
            giusController.StartMove();
        }

        // Effectが終わってから戦闘開始
        if (bossController != null)
        {
            bossController.SetBattleStarted(true);
        }

        appearCoroutine = null;

        Debug.Log(
            "Gius：登場Effect終了、移動・戦闘開始",
            this
        );
    }

    private void InitializeAppearEffect()
    {
        if (appearEffectRoot == null)
        {
            Debug.LogWarning(
                "Appear Effect Rootが設定されていません。",
                this
            );

            return;
        }

        appearEffectRoot.SetActive(true);

        appearParticles =
            appearEffectRoot.GetComponentsInChildren<ParticleSystem>(
                true
            );

        if (appearParticles == null ||
            appearParticles.Length == 0)
        {
            Debug.LogWarning(
                "Appear Effect Root内にParticle Systemがありません。",
                appearEffectRoot
            );

            return;
        }

        foreach (ParticleSystem particle in appearParticles)
        {
            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main =
                particle.main;

            // Time.timeScale = 0でも動かす
            main.useUnscaledTime = true;

            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        Debug.Log(
            $"Gius登場Effect初期化：Particle数 {appearParticles.Length}",
            appearEffectRoot
        );
    }

    private void PlayAppearEffect()
    {
        if (appearEffectRoot == null)
        {
            Debug.LogWarning(
                "Appear Effect Rootが設定されていません。",
                this
            );

            return;
        }

        appearEffectRoot.SetActive(true);

        // Effectをボスの登場位置へ合わせる
        appearEffectRoot.transform.position =
            boss.position;

        if (appearParticles == null ||
            appearParticles.Length == 0)
        {
            appearParticles =
                appearEffectRoot.GetComponentsInChildren<ParticleSystem>(
                    true
                );
        }

        if (appearParticles == null ||
            appearParticles.Length == 0)
        {
            Debug.LogWarning(
                "再生できるParticle Systemがありません。",
                appearEffectRoot
            );

            return;
        }

        foreach (ParticleSystem particle in appearParticles)
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
            $"Gius登場Effect再生：Particle数 {appearParticles.Length}",
            appearEffectRoot
        );
    }

    private void StopAppearEffect()
    {
        if (appearParticles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in appearParticles)
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

    private void RestoreTimeScale()
    {
        Time.timeScale = savedTimeScale;
    }

    private void OnDisable()
    {
        if (appearCoroutine != null)
        {
            StopCoroutine(appearCoroutine);
            appearCoroutine = null;
        }

        StopAppearEffect();

        if (appearing)
        {
            RestoreTimeScale();
        }

        appearing = false;
    }
}