
using System;
using System.Collections;
using UnityEngine;

public class CircusRoulette : MonoBehaviour
{
    // ========================================
    // ルーレットの結果
    // ========================================

    public enum RouletteResult
    {
        Jackpot,
        Heal,
        Miss,
        FakeBall,
        Reverse,
        Shrink,
        SmashLock,
        BigWin
    }

    // ========================================
    // ルーレット設定
    // ========================================

    [Header("回転する円盤")]
    [SerializeField]
    private Transform wheel;

    [Header("抽選設定")]
    [SerializeField, Min(0f)]
    private float interval = 20f;

    [SerializeField, Min(0.1f)]
    private float spinDuration = 3f;

    [SerializeField, Min(1)]
    private int fullRotations = 5;

    [Header("当選確率")]
    [Tooltip("Jackpot, Heal, Miss, FakeBall, Reverse, Shrink, SmashLock, BigWinの順")]
    [SerializeField]
    private float[] weights =
    {
        1f, 1f, 1f, 1f,
        1f, 1f, 1f, 1f
    };

    [Header("開始・停止")]
    [SerializeField]
    private bool playOnStart = true;

    // ========================================
    // 結果通知
    // ========================================

    public event Action<RouletteResult> OnResult;

    public bool IsSpinning { get; private set; }
    public bool IsPaused { get; private set; }

    private Coroutine routine;

    // ========================================
    // 画像のマス配置
    // ========================================

    // 12時方向の境界から時計回り
    // 0 Jackpot
    // 1 Heal
    // 2 Shrink
    // 3 FakeBall
    // 4 Miss
    // 5 Reverse
    // 6 BigWin
    // 7 SmashLock

    private readonly RouletteResult[] wheelOrder =
    {
        RouletteResult.Jackpot,
        RouletteResult.Heal,
        RouletteResult.Shrink,
        RouletteResult.FakeBall,
        RouletteResult.Miss,
        RouletteResult.Reverse,
        RouletteResult.BigWin,
        RouletteResult.SmashLock
    };

    // ========================================
    // 初期化
    // ========================================

    private void Start()
    {
        if (playOnStart)
        {
            StartRoulette();
        }
    }

    // ========================================
    // 開始・停止
    // ========================================

    public void StartRoulette()
    {
        if (routine == null && wheel != null)
        {
            routine = StartCoroutine(RouletteLoop());
        }
    }

    public void StopRoulette()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = null;
        IsSpinning = false;
    }

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
    }

    // ========================================
    // ルーレットループ
    // ========================================

    private IEnumerator RouletteLoop()
    {
        while (true)
        {
            float elapsed = 0f;

            while (elapsed < interval)
            {
                if (!IsPaused)
                {
                    elapsed += Time.deltaTime;
                }

                yield return null;
            }

            // 抽選する効果を決定
            RouletteResult result = DrawResult();

            // 画像内でのマス位置を取得
            int wheelIndex = GetWheelIndex(result);

            // 該当マスまで回転
            yield return SpinTo(wheelIndex);

            // 結果を通知
            OnResult?.Invoke(result);

            Debug.Log(
                $"ルーレット結果: {result}",
                this
            );
        }
    }

    // ========================================
    // 抽選処理
    // ========================================

    private RouletteResult DrawResult()
    {
        int index = DrawIndex();

        return (RouletteResult)index;
    }

    private int DrawIndex()
    {
        if (weights == null || weights.Length != 8)
        {
            return UnityEngine.Random.Range(0, 8);
        }

        float total = 0f;

        foreach (float weight in weights)
        {
            total += Mathf.Max(0f, weight);
        }

        if (total <= 0f)
        {
            return UnityEngine.Random.Range(0, 8);
        }

        float roll = UnityEngine.Random.value * total;

        for (int i = 0; i < 8; i++)
        {
            roll -= Mathf.Max(0f, weights[i]);

            if (roll < 0f)
            {
                return i;
            }
        }

        return 7;
    }

    // ========================================
    // 結果から画像の位置を取得
    // ========================================

    private int GetWheelIndex(RouletteResult result)
    {
        for (int i = 0; i < wheelOrder.Length; i++)
        {
            if (wheelOrder[i] == result)
            {
                return i;
            }
        }

        return 0;
    }

    // ========================================
    // 回転処理
    // ========================================

    private IEnumerator SpinTo(int index)
    {
        IsSpinning = true;

        float startAngle = wheel.localEulerAngles.z;

        float targetAngle =
            22.5f + index * 45f;

        // 時計回りに回転する距離
        float remainingClockwise =
            Mathf.Repeat(
                startAngle - targetAngle,
                360f
            );

        float totalDegrees =
            fullRotations * 360f +
            remainingClockwise;

        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            if (!IsPaused)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(
                    elapsed / spinDuration
                );

                // 最初は速く、最後はゆっくり停止
                float eased =
                    1f - Mathf.Pow(1f - t, 3f);

                wheel.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        startAngle -
                        totalDegrees * eased
                    );
            }

            yield return null;
        }

        // 最終角度を正確に合わせる
        wheel.localRotation =
            Quaternion.Euler(
                0f,
                0f,
                targetAngle
            );

        IsSpinning = false;
    }
}
