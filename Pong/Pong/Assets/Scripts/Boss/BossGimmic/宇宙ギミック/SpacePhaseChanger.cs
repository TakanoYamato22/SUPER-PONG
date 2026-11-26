
using System;
using System.Collections;
using UnityEngine;

public class SpacePhaseChanger : BossPhaseChangerBase
{
    [Header("暗転")]
    [SerializeField] private ScreenFader screenFader;

    [Header("カメラ復帰演出")]
    [SerializeField] private SpaceScreenRotator screenRotator;

    [SerializeField, Min(0.01f)]
    private float cameraReturnDuration = 2f;

    [Header("ボスの見た目")]
    [SerializeField] private GameObject firstBossVisual;
    [SerializeField] private GameObject secondBossVisual;

    [Header("背景")]
    [SerializeField] private GameObject firstBackground;
    [SerializeField] private GameObject secondBackground;

    [Header("紫色のエネルギー")]
    [SerializeField] private ParticleSystem energyParticle;

    [Header("震え演出")]
    [SerializeField, Min(0f)]
    private float shakeDuration = 1.5f;

    [SerializeField, Min(0f)]
    private float shakePower = 0.12f;

    [Header("エネルギー集中時間")]
    [SerializeField, Min(0f)]
    private float energyDuration = 2f;

    [Header("暗転維持時間")]
    [SerializeField, Min(0f)]
    private float blackScreenDuration = 0.3f;

    private bool phaseChanged;
    private bool phaseChanging;

    private Coroutine phaseCoroutine;

    private float previousTimeScale = 1f;
    private Vector3 originalVisualPosition;

    public override bool PhaseChanged => phaseChanged;
    public override bool PhaseChanging => phaseChanging;

    // ========================================
    // 初期化
    // ========================================

    private void Awake()
    {
        if (firstBossVisual != null)
        {
            firstBossVisual.SetActive(true);

            originalVisualPosition =
                firstBossVisual.transform.localPosition;
        }

        if (secondBossVisual != null)
            secondBossVisual.SetActive(false);

        if (firstBackground != null)
            firstBackground.SetActive(true);

        if (secondBackground != null)
            secondBackground.SetActive(false);

        StopEnergyParticle();
    }

    // ========================================
    // 形態変化開始
    // ========================================

    public override void StartPhaseChange(Action onFinished = null)
    {
        if (phaseChanged || phaseChanging)
            return;

        phaseCoroutine = StartCoroutine(
            PhaseChangeRoutine(onFinished)
        );
    }

    // ========================================
    // 形態変化演出
    // ========================================

    private IEnumerator PhaseChangeRoutine(Action onFinished)
    {
        phaseChanging = true;

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        Debug.Log("宇宙ボス：形態変化開始！");

        // ========================================
        // 1. カメラを元の向きに戻す
        // ========================================

        if (screenRotator != null)
        {
            yield return StartCoroutine(
                screenRotator.ReturnToOriginalRotation(
                    cameraReturnDuration
                )
            );
        }

        // ========================================
        // 2. ボスが震える
        // ========================================

        if (firstBossVisual != null)
        {
            Transform visual = firstBossVisual.transform;

            originalVisualPosition = visual.localPosition;

            float timer = 0f;

            while (timer < shakeDuration)
            {
                timer += Time.unscaledDeltaTime;

                Vector2 offset =
                    UnityEngine.Random.insideUnitCircle
                    * shakePower;

                visual.localPosition =
                    originalVisualPosition +
                    new Vector3(offset.x, offset.y, 0f);

                yield return null;
            }

            visual.localPosition = originalVisualPosition;
        }
        else
        {
            yield return new WaitForSecondsRealtime(
                shakeDuration
            );
        }

        // ========================================
        // 3. 紫色のエネルギーを集中
        // ========================================

        PlayEnergyParticle();

        yield return new WaitForSecondsRealtime(
            energyDuration
        );

        // ========================================
        // 4. 暗転
        // ========================================

        if (screenFader != null)
        {
            yield return StartCoroutine(
                screenFader.FadeToBlack()
            );
        }
        else
        {
            Debug.LogWarning(
                "ScreenFaderが設定されていません！",
                this
            );
        }

        StopEnergyParticle();

        // ========================================
        // 5. ボスと背景を切り替え
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
        // 6. 暗転状態を維持
        // ========================================

        yield return new WaitForSecondsRealtime(
            blackScreenDuration
        );

        // ========================================
        // 7. 明転
        // ========================================

        if (screenFader != null)
        {
            yield return StartCoroutine(
                screenFader.FadeFromBlack()
            );
        }

        // ========================================
        // 8. 第2形態開始
        // ========================================

        Time.timeScale = previousTimeScale;

        phaseChanged = true;
        phaseChanging = false;
        phaseCoroutine = null;

        // BossController側で第2形態HPを設定
        onFinished?.Invoke();

        Debug.Log("宇宙ボス：第2形態開始！");
    }

    // ========================================
    // Particle再生
    // ========================================

    private void PlayEnergyParticle()
    {
        if (energyParticle == null)
        {
            Debug.LogWarning(
                "紫色のParticleが未設定！",
                this
            );

            return;
        }

        GameObject effectObject = energyParticle.gameObject;

        if (!effectObject.activeSelf)
            effectObject.SetActive(true);

        if (!effectObject.activeInHierarchy)
        {
            Debug.LogWarning(
                "Particleの親オブジェクトが無効です！",
                effectObject
            );

            return;
        }

        var main = energyParticle.main;
        main.useUnscaledTime = true;

        energyParticle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        energyParticle.Play(true);
    }

    // ========================================
    // Particle停止
    // ========================================

    private void StopEnergyParticle()
    {
        if (energyParticle == null)
            return;

        energyParticle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }

    // ========================================
    // 自分自身の無効化防止
    // ========================================

    private bool WouldDisableThisComponent(GameObject target)
    {
        if (target == null)
            return false;

        return target == gameObject ||
               transform.IsChildOf(target.transform);
    }

    // ========================================
    // 無効化時の後片付け
    // ========================================

    private void OnDisable()
    {
        if (phaseChanging)
            Time.timeScale = previousTimeScale;

        if (phaseCoroutine != null)
        {
            StopCoroutine(phaseCoroutine);
            phaseCoroutine = null;
        }

        if (firstBossVisual != null)
        {
            firstBossVisual.transform.localPosition =
                originalVisualPosition;
        }

        StopEnergyParticle();

        phaseChanging = false;
    }
}
