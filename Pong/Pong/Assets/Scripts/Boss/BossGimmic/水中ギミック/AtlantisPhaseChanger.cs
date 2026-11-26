
using System;
using System.Collections;
using UnityEngine;

public class AtlantisPhaseChanger : BossPhaseChangerBase
{
    [Header("暗転")]
    [SerializeField] private ScreenFader screenFader;

    [Header("ボスの見た目")]
    [SerializeField] private GameObject firstBossVisual;
    [SerializeField] private GameObject secondBossVisual;

    [Header("背景")]
    [SerializeField] private GameObject firstBackground;
    [SerializeField] private GameObject secondBackground;

    [Header("第2形態変化エフェクト")]
    [SerializeField] private ParticleSystem phaseChangeEffect;

    [Header("進化前の演出時間")]
    [SerializeField, Min(0f)]
    private float effectDuration = 2f;

    [Header("暗転時間")]
    [SerializeField, Min(0f)]
    private float blackScreenDuration = 0.3f;

    private bool phaseChanged;
    private bool phaseChanging;
    private Coroutine phaseCoroutine;

    private float previousTimeScale = 1f;

    public override bool PhaseChanged => phaseChanged;
    public override bool PhaseChanging => phaseChanging;

    private void Awake()
    {
        if (firstBossVisual != null)
            firstBossVisual.SetActive(true);

        if (secondBossVisual != null)
            secondBossVisual.SetActive(false);

        if (firstBackground != null)
            firstBackground.SetActive(true);

        if (secondBackground != null)
            secondBackground.SetActive(false);

        if (phaseChangeEffect != null)
        {
            phaseChangeEffect.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }
    }

    public override void StartPhaseChange(Action onFinished = null)
    {
        if (phaseChanged || phaseChanging)
            return;

        phaseCoroutine = StartCoroutine(
            PhaseChangeRoutine(onFinished)
        );
    }

    private IEnumerator PhaseChangeRoutine(Action onFinished)
    {
        phaseChanging = true;

        previousTimeScale = Time.timeScale;

        // 戦闘停止
        Time.timeScale = 0f;

        // ========================================
        // 1. 進化前のキラキラ演出
        // ========================================

        PlayPhaseChangeEffect();

        yield return new WaitForSecondsRealtime(
            effectDuration
        );

        // ========================================
        // 2. 暗転
        // ========================================

        if (screenFader != null)
        {
            yield return StartCoroutine(
                screenFader.FadeToBlack()
            );
        }

        StopPhaseChangeEffect();

        // ========================================
        // 3. ボスと背景を切り替え
        // ========================================

        if (secondBossVisual != null)
            secondBossVisual.SetActive(true);

        if (secondBackground != null)
            secondBackground.SetActive(true);

        if (firstBossVisual != null &&
            !WouldDisableThisComponent(firstBossVisual))
        {
            firstBossVisual.SetActive(false);
        }

        if (firstBackground != null &&
            !WouldDisableThisComponent(firstBackground))
        {
            firstBackground.SetActive(false);
        }

        // ========================================
        // 4. 暗転状態を維持
        // ========================================

        yield return new WaitForSecondsRealtime(
            blackScreenDuration
        );

        // ========================================
        // 5. 明転
        // ========================================

        if (screenFader != null)
        {
            yield return StartCoroutine(
                screenFader.FadeFromBlack()
            );
        }

        // ========================================
        // 6. 戦闘再開
        // ========================================

        Time.timeScale = previousTimeScale;

        phaseChanged = true;
        phaseChanging = false;
        phaseCoroutine = null;

        // 第2形態のHPを設定して戦闘再開
        onFinished?.Invoke();

        // ギミック開始はAtlantisBossControllerで
        // HPを監視して管理する

        Debug.Log("Atlantis：第2形態開始！");
    }

    private void PlayPhaseChangeEffect()
    {
        if (phaseChangeEffect == null)
        {
            Debug.LogWarning(
                "キラキラのParticleが未設定！",
                this
            );
            return;
        }

        GameObject effectObject =
            phaseChangeEffect.gameObject;

        if (!effectObject.activeSelf)
        {
            effectObject.SetActive(true);
        }

        if (!effectObject.activeInHierarchy)
        {
            Debug.LogWarning(
                "キラキラの親オブジェクトが無効です！",
                effectObject
            );
            return;
        }

        var main = phaseChangeEffect.main;

        main.useUnscaledTime = true;

        phaseChangeEffect.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        phaseChangeEffect.Play(true);

        Debug.Log(
            "進化前のキラキラ開始！",
            phaseChangeEffect
        );
    }

    private void StopPhaseChangeEffect()
    {
        if (phaseChangeEffect == null)
            return;

        phaseChangeEffect.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }

    private bool WouldDisableThisComponent(GameObject target)
    {
        if (target == null)
            return false;

        return target == gameObject ||
               transform.IsChildOf(target.transform);
    }

    private void OnDisable()
    {
        if (phaseChanging)
        {
            Time.timeScale = previousTimeScale;
        }

        if (phaseCoroutine != null)
        {
            StopCoroutine(phaseCoroutine);
            phaseCoroutine = null;
        }

        StopPhaseChangeEffect();

        phaseChanging = false;
    }
}
