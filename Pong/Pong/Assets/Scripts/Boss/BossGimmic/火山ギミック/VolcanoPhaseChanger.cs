using System;
using System.Collections;
using UnityEngine;

public class VolcanoPhaseChanger :
    BossPhaseChangerBase
{
    [Header("火山Boss制御")]
    [SerializeField]
    private VolcanoBossController bossController;

    [Header("暗転")]
    [SerializeField]
    private ScreenFader screenFader;

    [Header("背景")]
    [Tooltip("第1形態の背景")]
    [SerializeField]
    private GameObject normalBackground;

    [Tooltip("第2形態の背景")]
    [SerializeField]
    private GameObject mixedBackground;

    [Header("Bossの見た目")]
    [Tooltip(
        "Dragon本体ではなく、" +
        "第1形態のSpriteを持つ子オブジェクトを設定"
    )]
    [SerializeField]
    private GameObject normalBossVisual;

    [Tooltip(
        "第2形態のSpriteを持つ" +
        "子オブジェクトを設定"
    )]
    [SerializeField]
    private GameObject mixedBossVisual;

    [Header("Paddle")]
    [SerializeField]
    private Paddle[] paddles;

    [Header("演出時間")]
    [Tooltip("Bossが震える時間")]
    [Min(0f)]
    [SerializeField]
    private float shakeDuration = 1f;

    [Tooltip("真っ暗な状態を維持する時間")]
    [Min(0f)]
    [SerializeField]
    private float blackScreenDuration = 0.2f;

    private bool phaseChanged;
    private bool phaseChanging;

    private Action onPhaseChangeFinished;

    private Coroutine phaseChangeCoroutine;

    public override bool PhaseChanged =>
        phaseChanged;

    public override bool PhaseChanging =>
        phaseChanging;

    private void Awake()
    {
        FindReferences();
        SetInitialVisualState();
    }

    /// <summary>
    /// 火山Bossの形態変化を開始する。
    /// </summary>
    public override void StartPhaseChange(
        Action onFinished = null
    )
    {
        if (phaseChanged ||
            phaseChanging)
        {
            Debug.LogWarning(
                "形態変化は既に開始済みです。",
                this
            );

            return;
        }

        onPhaseChangeFinished =
            onFinished;

        phaseChangeCoroutine =
            StartCoroutine(
                PhaseChangeRoutine()
            );
    }

    private IEnumerator PhaseChangeRoutine()
    {
        phaseChanging = true;

        float previousTimeScale =
            Time.timeScale;

        // ゲーム全体を停止
        Time.timeScale = 0f;

        /*
         * 1. Bossを震わせる
         *
         * VolcanoBossController側は
         * unscaledDeltaTimeを使うので、
         * Time.timeScaleが0でも動く。
         */
        if (bossController != null &&
            shakeDuration > 0f)
        {
            yield return StartCoroutine(
                bossController
                    .PlayPhaseChangeShake(
                        shakeDuration
                    )
            );
        }

        /*
         * 2. 暗転
         */
        if (screenFader != null)
        {
            yield return StartCoroutine(
                screenFader.FadeToBlack()
            );
        }
        else
        {
            Debug.LogWarning(
                "ScreenFaderが設定されていません。",
                this
            );
        }

        /*
         * 3. 画面が真っ暗な間に、
         * 背景とBossの見た目を変更。
         */
        ChangeVisuals();

        if (blackScreenDuration > 0f)
        {
            yield return
                new WaitForSecondsRealtime(
                    blackScreenDuration
                );
        }

        /*
         * 4. 明転
         */
        if (screenFader != null)
        {
            yield return StartCoroutine(
                screenFader.FadeFromBlack()
            );
        }

        /*
         * 5. 元の時間速度に戻す
         */
        Time.timeScale =
            previousTimeScale;

        phaseChanged = true;
        phaseChanging = false;
        phaseChangeCoroutine = null;

        Action callback =
            onPhaseChangeFinished;

        onPhaseChangeFinished = null;

        callback?.Invoke();

        if (bossController != null)
        {
            bossController
                .StartMoveAfterPhaseChange();
        }

        EnablePaddleInertia();

        Debug.Log(
            "火山Boss：第2形態への移行完了",
            this
        );
    }

    private void FindReferences()
    {
        if (bossController == null)
        {
            bossController =
                GetComponent
                    <VolcanoBossController>();
        }

        if (bossController == null)
        {
            bossController =
                GetComponentInParent
                    <VolcanoBossController>();
        }

        if (bossController == null)
        {
            bossController =
                GetComponentInChildren
                    <VolcanoBossController>(
                        true
                    );
        }
    }

    private void SetInitialVisualState()
    {
        if (normalBackground != null)
        {
            normalBackground.SetActive(
                true
            );
        }

        if (mixedBackground != null)
        {
            mixedBackground.SetActive(
                false
            );
        }

        if (normalBossVisual != null)
        {
            normalBossVisual.SetActive(
                true
            );
        }

        if (mixedBossVisual != null)
        {
            mixedBossVisual.SetActive(
                false
            );
        }
    }

    private void ChangeVisuals()
    {
        if (WouldDisableThisComponent(
                normalBossVisual
            ))
        {
            Debug.LogError(
                "Normal Boss VisualにBoss本体が" +
                "設定されています。" +
                "第1形態の見た目だけを持つ" +
                "子オブジェクトを設定してください。",
                this
            );

            return;
        }

        if (WouldDisableThisComponent(
                normalBackground
            ))
        {
            Debug.LogError(
                "Normal Backgroundを無効にすると、" +
                "VolcanoPhaseChangerまで停止します。" +
                "背景用の別オブジェクトを設定してください。",
                this
            );

            return;
        }

        // 新しい見た目を先に表示
        if (mixedBackground != null)
        {
            mixedBackground.SetActive(
                true
            );
        }

        if (mixedBossVisual != null)
        {
            mixedBossVisual.SetActive(
                true
            );
        }

        // 古い見た目だけ非表示
        if (normalBackground != null)
        {
            normalBackground.SetActive(
                false
            );
        }

        if (normalBossVisual != null)
        {
            normalBossVisual.SetActive(
                false
            );
        }

        Debug.Log(
            "背景とBossの見た目を切り替えました。",
            this
        );
    }

    private bool WouldDisableThisComponent(
        GameObject target
    )
    {
        if (target == null)
        {
            return false;
        }

        if (target == gameObject)
        {
            return true;
        }

        return transform.IsChildOf(
            target.transform
        );
    }

    private void EnablePaddleInertia()
    {
        if (paddles == null)
        {
            return;
        }

        foreach (Paddle paddle in paddles)
        {
            if (paddle == null)
            {
                continue;
            }

            paddle.EnableInertia();
        }
    }

    private void OnDisable()
    {
        if (phaseChanging &&
            Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        if (phaseChangeCoroutine != null)
        {
            StopCoroutine(
                phaseChangeCoroutine
            );

            phaseChangeCoroutine = null;
        }

        phaseChanging = false;
        onPhaseChangeFinished = null;
    }
}