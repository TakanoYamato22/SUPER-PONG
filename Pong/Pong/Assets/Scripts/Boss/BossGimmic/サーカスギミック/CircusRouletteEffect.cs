using UnityEngine;

public class CircusRouletteEffect : MonoBehaviour
{
    // ========================================
    // 参照
    // ========================================

    [Header("ルーレット")]
    [SerializeField]
    private CircusRoulette roulette;

    [Header("サーカスボス")]
    [SerializeField]
    private CircusBossController circusBoss;

    [Header("プレイヤーHP")]
    [SerializeField]
    private PlayerHealth player1Health;

    [SerializeField]
    private PlayerHealth player2Health;

    [Header("Paddle")]
    [SerializeField]
    private Paddle player1Paddle;

    [SerializeField]
    private Paddle player2Paddle;

    [Header("SmashController")]
    [SerializeField]
    private SmashController player1Smash;

    [SerializeField]
    private SmashController player2Smash;

    [Header("FakeBall")]
    [SerializeField]
    private CircusFakeBallSpawner fakeBallSpawner;

    // ========================================
    // 効果設定
    // ========================================

    [Header("回復設定")]
    [SerializeField, Min(0f)]
    private float healAmount = 25f;

    [Header("操作反転設定")]
    [SerializeField, Min(0f)]
    private float reverseDuration = 10f;

    [Header("Paddle縮小設定")]
    [SerializeField, Min(0f)]
    private float shrinkDuration = 10f;

    [Header("スマッシュ封印設定")]
    [SerializeField, Min(0f)]
    private float smashLockDuration = 10f;

    // ========================================
    // イベント登録
    // ========================================

    private void OnEnable()
    {
        if (roulette != null)
        {
            roulette.OnResult += ApplyResult;
        }
    }

    private void OnDisable()
    {
        if (roulette != null)
        {
            roulette.OnResult -= ApplyResult;
        }

        StopReverse();
        StopSmashLock();
    }

    // ========================================
    // ルーレット結果
    // ========================================

    private void ApplyResult(
        CircusRoulette.RouletteResult result
    )
    {
        switch (result)
        {
            case CircusRoulette.RouletteResult.Jackpot:
                ActivateJackpot();
                break;

            case CircusRoulette.RouletteResult.Heal:
                HealPlayers();
                break;

            case CircusRoulette.RouletteResult.Miss:
                Debug.Log(
                    "【Circus】ハズレ！何も起こらない！"
                );
                break;

            case CircusRoulette.RouletteResult.BigWin:
                HealPlayers();
                ActivateJackpot();

                Debug.Log(
                    "【Circus】大当たり！回復＋ジャックポット！"
                );
                break;

            case CircusRoulette.RouletteResult.FakeBall:
                SpawnFakeBall();
                break;

            case CircusRoulette.RouletteResult.Reverse:
                ReversePlayers();
                break;

            case CircusRoulette.RouletteResult.Shrink:
                ShrinkPlayers();
                break;

            case CircusRoulette.RouletteResult.SmashLock:
                LockPlayersSmash();
                break;
        }
    }

    // ========================================
    // ジャックポット
    // ========================================

    private void ActivateJackpot()
    {
        if (circusBoss == null)
        {
            Debug.LogWarning(
                "【Circus】CircusBossControllerが未設定です！",
                this
            );

            return;
        }

        circusBoss.ActivateJackpot();

        Debug.Log(
            "【Circus】ジャックポット発動！"
        );
    }

    // ========================================
    // 両プレイヤー回復
    // ========================================

    private void HealPlayers()
    {
        if (player1Health == null &&
            player2Health == null)
        {
            Debug.LogWarning(
                "【Circus】PlayerHealthが設定されていません！",
                this
            );

            return;
        }

        if (player1Health != null)
        {
            player1Health.Heal(healAmount);
        }

        if (player2Health != null &&
            player2Health != player1Health)
        {
            player2Health.Heal(healAmount);
        }

        Debug.Log(
            $"【Circus】HP回復！回復量：{healAmount}"
        );
    }

    // ========================================
    // FakeBall生成
    // ========================================

    private void SpawnFakeBall()
    {
        if (fakeBallSpawner == null)
        {
            Debug.LogWarning(
                "【Circus】FakeBallSpawnerが未設定です！",
                this
            );

            return;
        }

        fakeBallSpawner.SpawnFakeBall();
    }

    // ========================================
    // 操作反転
    // ========================================

    private void ReversePlayers()
    {
        if (player1Paddle == null &&
            player2Paddle == null)
        {
            Debug.LogWarning(
                "【Circus】Paddleが設定されていません！",
                this
            );

            return;
        }

        if (player1Paddle != null)
        {
            player1Paddle.Reverse(reverseDuration);
        }

        if (player2Paddle != null &&
            player2Paddle != player1Paddle)
        {
            player2Paddle.Reverse(reverseDuration);
        }

        Debug.Log(
            $"【Circus】操作反転！{reverseDuration}秒間"
        );
    }

    private void StopReverse()
    {
        if (player1Paddle != null)
        {
            player1Paddle.StopReverse();
        }

        if (player2Paddle != null &&
            player2Paddle != player1Paddle)
        {
            player2Paddle.StopReverse();
        }
    }

    // ========================================
    // Paddle縮小
    // ========================================

    private void ShrinkPlayers()
    {
        if (player1Paddle == null &&
            player2Paddle == null)
        {
            Debug.LogWarning(
                "【Circus】Paddleが設定されていません！",
                this
            );

            return;
        }

        if (player1Paddle != null)
        {
            player1Paddle.Shrink(shrinkDuration);
        }

        if (player2Paddle != null &&
            player2Paddle != player1Paddle)
        {
            player2Paddle.Shrink(shrinkDuration);
        }

        Debug.Log(
            $"【Circus】Paddle縮小！{shrinkDuration}秒間"
        );
    }

    // ========================================
    // スマッシュ封印
    // ========================================

    private void LockPlayersSmash()
    {
        if (player1Smash == null &&
            player2Smash == null)
        {
            Debug.LogWarning(
                "【Circus】SmashControllerが設定されていません！",
                this
            );

            return;
        }

        if (player1Smash != null)
        {
            player1Smash.LockSmash(smashLockDuration);
        }

        if (player2Smash != null &&
            player2Smash != player1Smash)
        {
            player2Smash.LockSmash(smashLockDuration);
        }

        Debug.Log(
            $"【Circus】スマッシュ封印！{smashLockDuration}秒間"
        );
    }

    private void StopSmashLock()
    {
        if (player1Smash != null)
        {
            player1Smash.UnlockSmash();
        }

        if (player2Smash != null &&
            player2Smash != player1Smash)
        {
            player2Smash.UnlockSmash();
        }
    }
}